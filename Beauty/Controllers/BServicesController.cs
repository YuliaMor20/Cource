using System.Linq;
using System.Threading.Tasks;
using Beauty.Models;
using Beauty.Repository;
using Beauty.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace Beauty.Controllers
{
    public class BServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

     

        // GET: BServices

        public IActionResult Index()
        {
            var bServices = _context.BServices
                .Include(x => x.TypeService)
                .ToList(); // Retrieve BServices from the database

            var bServiceViewModels = bServices.Select(model => new BServiceViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                Time = model.Time,
                Description = model.Description,
                Price = model.Price,
                TypeServiceId = model.TypeServiceId,
                TypeService = model.TypeService,
            });

            return View(bServiceViewModels); // Pass BServiceViewModels to the view
        }

        // GET: BServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bService = await _context.BServices
                .Include(b => b.TypeService)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (bService == null)
            {
                return NotFound();
            }

            return View(bService);
        }
        public IActionResult Create()
        {
            // Populate the ViewBag.TypeService with available TypeServices.
            ViewBag.TypeService = new SelectList(_context.TypeServices, "Id", "Title");
            return View();
        }

        // POST: BServices/Create
        [HttpPost]
        public async Task<IActionResult> Create(BServiceViewModel vm)
        {
            // If the ModelState is not valid, we need to set up the SelectList again.
            // Populate the ViewBag.TypeService with available TypeServices.
            ViewBag.TypeService = new SelectList(_context.TypeServices, "Id", "Title", vm.TypeServiceId);

            if (ModelState.IsValid)
            {
                var bService = new BService();

                if (vm.ImageFile != null && vm.ImageFile.Length > 0)
                {
                    byte[] photo = null;

                    using (var fileStream = vm.ImageFile.OpenReadStream())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            fileStream.CopyTo(memoryStream);
                            photo = memoryStream.ToArray();
                        }
                    }

                    bService.Image = photo;
                }

                bService.Price = vm.Price;
                bService.Time = vm.Time;
                bService.Description = vm.Description;
                bService.Title = vm.Title;
                bService.TypeServiceId = vm.TypeServiceId;
                bService.TypeService = vm.TypeService;

                _context.BServices.Add(bService);
                await _context.SaveChangesAsync(); // Added await here

                return RedirectToAction("Index");
            }

            return View(vm);
        }

        private int GetBServiceTimeById(int bServiceId)
        {
            var bService = _context.BServices.FirstOrDefault(b => b.Id == bServiceId);
            return bService?.Time ?? 0; // Возвращает время услуги в минутах, или 0, если не найдено
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _context.BServices
                .Include(x => x.TypeService)
                .FirstOrDefault(x => x.Id == id);

            ViewBag.TypeService = new SelectList(_context.TypeServices, "Id", "Title", item?.TypeServiceId);

            // Добавление времени услуги в ViewBag
            ViewBag.BServiceTime = GetBServiceTimeById(item?.Id ?? 0);

            return View(item);
        }

        // POST: BServices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Time,Image,Price,TypeServiceId,MasterId")] BService bService)
        {
            if (id != bService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Получение времени услуги
                    var bServiceTime = GetBServiceTimeById(bService.Id);

                    // Рассчет времени окончания услуги
                    var serviceEndTime = DateTime.UtcNow.AddMinutes(bServiceTime);

                    // Проверка, не превышает ли выбранное время + время услуги 22:00
                    if (serviceEndTime > new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 22, 0, 0))
                    {
                        ModelState.AddModelError("Time", "The selected service time exceeds the available hours. Please choose a different time.");
                        ViewBag.TypeService = new SelectList(_context.TypeServices, "Id", "Title", bService.TypeServiceId);
                        return View(bService);
                    }

                    _context.Update(bService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BServiceExists(bService.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeServiceId"] = new SelectList(_context.TypeServices, "Id", "Title", bService.TypeServiceId);
            return View(bService);
        }

        // GET: BServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bService = await _context.BServices
                .Include(b => b.TypeService)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (bService == null)
            {
                return NotFound();
            }

            return View(bService);
        }

        // POST: BServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bService = await _context.BServices.FindAsync(id);
            _context.BServices.Remove(bService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BServiceExists(int id)
        {
            return _context.BServices.Any(e => e.Id == id);
        }


    }
}
