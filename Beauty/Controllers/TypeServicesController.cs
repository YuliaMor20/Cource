using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Beauty.Models;
using Beauty.Repository;

namespace Beauty.Controllers
{
    public class TypeServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypeServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TypeService
        public IActionResult Index()
        {
            if (User.IsInRole("ADMIN") || User.IsInRole("MANAGER"))
            {
                return View(_context.TypeServices.ToList());
        }else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
                }

        }

        // GET: TypeService/Details/5
        public IActionResult Details(int? id)
        {
            if (User.IsInRole("ADMIN") || User.IsInRole("MANAGER"))
            {
                if (id == null)
            {
                return NotFound();
            }

            var typeService = _context.TypeServices
                .FirstOrDefault(ts => ts.Id == id);

            if (typeService == null)
            {
                return NotFound();
            }

            return View(typeService);
        }
            else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
            }

        }

        // GET: TypeService/Create
        public IActionResult Create()
        {
            if (User.IsInRole("ADMIN") || User.IsInRole("MANAGER"))
            {
                return View();
        }else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
             }


        }

        // POST: TypeService/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title")] TypeService typeService)
        {
            if (User.IsInRole("ADMIN"))
            {
                if (ModelState.IsValid)
            {
                _context.Add(typeService);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(typeService);
            }
            else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
            }


        }

        // GET: TypeService/Edit/5
        public IActionResult Edit(int? id)
        {
            if (User.IsInRole("ADMIN"))
            {
                if (id == null)
            {
                return NotFound();
            }

            var typeService = _context.TypeServices
                .FirstOrDefault(ts => ts.Id == id);

            if (typeService == null)
            {
                return NotFound();
            }

            return View(typeService);
        }
            else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
            }


        }

        // POST: TypeService/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title")] TypeService typeService)
        {
            if (User.IsInRole("ADMIN"))
            {
                if (id != typeService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeService);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeServiceExists(typeService.Id))
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
            return View(typeService);
        }else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
    }


}

// GET: TypeService/Delete/5
public IActionResult Delete(int? id)
        {
            if (User.IsInRole("ADMIN"))
            {
                if (id == null)
            {
                return NotFound();
            }

            var typeService = _context.TypeServices
                .FirstOrDefault(ts => ts.Id == id);

            if (typeService == null)
            {
                return NotFound();
            }

            return View(typeService);
        }
            else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
            }


        }

        // POST: TypeService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (User.IsInRole("ADMIN"))
            {
                var typeService = _context.TypeServices
                .FirstOrDefault(ts => ts.Id == id);

            _context.TypeServices.Remove(typeService);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
    }


}

private bool TypeServiceExists(int id)
        {
            return _context.TypeServices.Any(ts => ts.Id == id);
        }
    }
}