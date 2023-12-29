using Beauty.Models;
using Beauty.Repositories;
using Beauty.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Beauty.Controllers
{
    public class RecordController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<BServicesController> _logger;
        public RecordController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor,
           UserManager<IdentityUser> userManager, ILogger<RecordController> logger)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        // GET: Record
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("ADMIN") || User.IsInRole("MANAGER"))
            {
                var records = await _context.Records.ToListAsync();
                return View(records);
            }
            else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
            }
        }
    

        // GET: Record/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (User.IsInRole("ADMIN") || User.IsInRole("MANAGER"))
            {
                if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Records
                .FirstOrDefaultAsync(m => m.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }
            else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
            }
        }

        public IActionResult Create(int bServiceId)
        {
            if (bServiceId == 0)
            {
                return NotFound();
            }

            // Get the type of service for the given BServiceId
            var bServiceType = _context.BServices
                .Where(b => b.Id == bServiceId)
                .Select(b => b.TypeService)
                .FirstOrDefault();

            // Get the masters with matching TypeService
            var mastersWithMatchingType = _context.Masters
                .Where(m => m.TypeService == bServiceType)
                .ToList();

            // Populate SelectList for MasterId
            ViewBag.MasterId = new SelectList(mastersWithMatchingType, "Id", "Title");

            // Populate SelectList for BService
            ViewBag.BServiceId = new SelectList(_context.BServices, "Id", "Title", bServiceId);

            // Set UserId using the GetUserId method
            string userId = GetUserId();

            // Initialize the Record with UserId
            var record = new Record
            {
                UserId = GetUserId(),
                BServiceId = bServiceId,
                CreateDateTime = DateTime.UtcNow // Set the default date and time
            };

            // Set the maximum time for service end
            ViewBag.MaxServiceEndTime = DateTime.Today.AddHours(21).AddMinutes(30); // Assuming today's date

            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId, BServiceId, MasterId, CreateDateTime")] Record record)
        {
            DateTime serviceEndTime = DateTime.MinValue; // Declare serviceEndTime here

            try
            {
                // Perform client-side validation
                var bService = await _context.BServices.FirstOrDefaultAsync(b => b.Id == record.BServiceId);

                if (bService == null)
                {
                    ModelState.AddModelError("BServiceId", "Invalid BService selected.");
                    PopulateSelectLists(record);

                    // Notify the user using JavaScript alert
                    string script = "alert('Invalid BService selected. Please choose a valid BService.');";
                    ViewBag.NotificationScript = script;

                    return View(record);
                }

                var selectedDateTime = record.CreateDateTime;

                // Calculate the service end time
                serviceEndTime = selectedDateTime.AddMinutes(bService.Time);

                // Check if the service end time exceeds the available hours
                if (serviceEndTime.TimeOfDay > new TimeSpan(22, 0, 0)) // Assuming 22:00 is the maximum time
                {
                    ModelState.AddModelError("CreateDateTime", "The selected service time exceeds the available hours. Please choose a different time.");
                    PopulateSelectLists(record);

                    // Notify the user using JavaScript alert
                    string script = "alert('The selected service time exceeds the available hours. Please choose a different time.');";
                    ViewBag.NotificationScript = script;

                    return View(record);
                }

                // Check if the selected master has overlapping appointments
                var existingAppointments = await _context.Records
                    .Where(r => r.MasterId == record.MasterId &&
                                r.CreateDateTime < serviceEndTime && // Existing appointment ends after the new appointment starts
                                r.CreateDateTime.AddMinutes(bService.Time) > record.CreateDateTime) // Existing appointment starts before the new appointment ends
                    .ToListAsync();

                if (existingAppointments.Any())
                {
                    ModelState.AddModelError("CreateDateTime", "The selected master has overlapping appointments. Please choose a different time.");
                    PopulateSelectLists(record);

                    // Notify the user using JavaScript alert
                    string script = "alert('The selected master has overlapping appointments. Please choose a different time.');";
                    ViewBag.NotificationScript = script;

                    return View(record);
                }

                // Client-side and server-side validation passed, proceed with saving the record

                if (ModelState.IsValid)
                {
                    _context.Add(record);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            // If ModelState is not valid or an exception occurred, redisplay the form with the entered data
            PopulateSelectLists(record);
            return View(record);
        }
        [HttpGet]
        public JsonResult CheckOverlappingAppointments(DateTime startTime, DateTime endTime, int masterId)
        {
            try
            {
                var overlappingAppointments = _context.Records
                    .Any(r => r.MasterId == masterId &&
                              r.CreateDateTime < endTime &&
                              r.CreateDateTime.AddMinutes(r.BService.Time) > startTime);

                return Json(new { hasOverlappingAppointments = overlappingAppointments });
            }
            catch (Exception ex)
            {
                return Json(new { hasOverlappingAppointments = false, message = ex.Message });
            }
        }
        private void PopulateSelectLists(Record record)
        {
            // Populate SelectList for MasterId
            ViewBag.MasterId = new SelectList(_context.Masters, "Id", "Title", record.MasterId);

            // Populate SelectList for BService
            ViewBag.BServiceId = new SelectList(_context.BServices, "Id", "Title", record.BServiceId);
        }
        // GET: Record/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = _context.Records.FirstOrDefault(r => r.Id == id);

            if (record == null)
            {
                return NotFound();
            }

            // Заполнение SelectList для выбора мастера
            ViewBag.MasterId = new SelectList(_context.Masters, "Id", "Title", record.MasterId);

            return View(record);
        }

        // ... Другие методы действий CRUD ...

        private bool RecordExists(int id)
        {
            return _context.Records.Any(e => e.Id == id);
        }


        [HttpGet]
        public JsonResult GetBServiceTime(int bServiceId)
        {
            try
            {
                var bService = _context.BServices.FirstOrDefault(b => b.Id == bServiceId);

                if (bService != null)
                {
                    return Json(new { success = true, time = bService.Time });
                }

                return Json(new { success = false, message = "BService not found." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
        public async Task<IActionResult> UserRecords()
        {
            try
            {
                // Получаем идентификатор текущего пользователя
                string userId = GetUserId();

                // Извлекаем записи из базы данных для текущего пользователя с загрузкой связанных данных
                var userRecords = await _context.Records
                    .Where(r => r.UserId == userId)
                    .Include(r => r.BService) // Загружаем связанный объект BService
                    .ToListAsync();

                // Возвращаем представление с записями пользователя
                return View(userRecords);
            }
            catch (Exception ex)
            {
                // Обработка ошибок, например, логирование
                _logger.LogError($"Error: {ex.Message}");
                return View("Error"); // Создайте представление "Error.cshtml" для отображения ошибки
            }
        }


    }
}