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
            return View(_context.TypeServices.ToList());
        }

        // GET: TypeService/Details/5
        public IActionResult Details(int? id)
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

        // GET: TypeService/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeService/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title")] TypeService typeService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeService);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(typeService);
        }

        // GET: TypeService/Edit/5
        public IActionResult Edit(int? id)
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

        // POST: TypeService/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title")] TypeService typeService)
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
        }

        // GET: TypeService/Delete/5
        public IActionResult Delete(int? id)
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

        // POST: TypeService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var typeService = _context.TypeServices
                .FirstOrDefault(ts => ts.Id == id);

            _context.TypeServices.Remove(typeService);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private bool TypeServiceExists(int id)
        {
            return _context.TypeServices.Any(ts => ts.Id == id);
        }
    }
}