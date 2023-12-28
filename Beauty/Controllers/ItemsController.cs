using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty.Models;
using Beauty.Repository;
using Beauty.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Beauty.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index(string sortOrder)
        {
            if (User.IsInRole("ADMIN") || User.IsInRole("MANAGER"))
            {
               

            ViewData["AvailabilitySort"] = String.IsNullOrEmpty(sortOrder) ? "availability_desc" : "";

            var items = _context.Items
                .Include(x => x.Category)
                .Select(model => new ItemViewModel()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    Category = model.Category,
                    Availability = model.Availability
                });

            switch (sortOrder)
            {
                case "availability_desc":
                    items = items.OrderByDescending(s => s.Availability);
                    break;
                default:
                    items = items.OrderBy(s => s.Availability);
                    break;
            }

            return View(items.ToList());
            }
            else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
            }
        }


        [HttpGet]
        public IActionResult Create()
        {
            if (User.IsInRole("ADMIN"))
            {
                ViewBag.Category = new SelectList(_context.Categories, "Id", "Title");

                return View();
            }

            else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
            }

            } 
        

        [HttpPost]
        public async Task<IActionResult> Create(ItemViewModel vm)
        {
        if (User.IsInRole("ADMIN"))
        {
            if (ModelState.IsValid)
            {
                var model = new Item();

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

                    model.Image = photo;
                }

                model.Price = vm.Price;
                model.Description = vm.Description;
                model.Title = vm.Title;
                model.CategoryId = vm.CategoryId;
                model.Availability = vm.Availability;

                _context.Items.Add(model);
                await _context.SaveChangesAsync(); // Добавлен оператор await здесь

                return RedirectToAction("Index");
            }

            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title");

            return View(vm);
        }

        else
        {
            // Пользователь не является администратором
            return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
        }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (User.IsInRole("ADMIN"))
            {
                var item = _context.Items
                .Include(x => x.Category)
                .FirstOrDefault(x => x.Id == id);

                ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", item?.CategoryId);

                return View(item);
            }
            else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
            }
            }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (User.IsInRole("ADMIN"))
            {
                var item = _context.Items.Find(id);

                if (item == null)
                {
                    return NotFound();
                }

                return View(item);
            }
            else
            {
               
                return RedirectToAction("AccessDenied", "Account"); 
            }
         }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (User.IsInRole("ADMIN"))
            {
                var item = _context.Items.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            _context.SaveChanges();

            return RedirectToAction("Index");
            }
            else
            {
                
                return RedirectToAction("AccessDenied", "Account"); 
            }
        }

        [HttpGet]
        public IActionResult OrderOnWarehouse(int id)

        {
            if (User.IsInRole("ADMIN") || User.IsInRole("MANAGER"))
            {


                var item = _context.Items.Find(id);

                if (item == null)
                {
                    return NotFound();
                }

                return View(item);
            }
            else
            {

                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        public IActionResult OrderOnWarehouse(int id, int quantity)
        {
            if (User.IsInRole("ADMIN") || User.IsInRole("MANAGER"))
            {
                var item = _context.Items.Find(id);

                if (item == null)
                {
                    return NotFound();
                }

                item.Availability += quantity;
                _context.SaveChanges();

                TempData["OrderOnWarehouseConfirmation"] = $"Заказ на склад успешно оформлен. Количество: {quantity}";

                return RedirectToAction("Index");
            }
            else
            {

                return RedirectToAction("AccessDenied", "Account");
            }
            }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var item = _context.Items
                .Include(x => x.Category)
                .FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

    }
}