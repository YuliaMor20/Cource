using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Beauty.Models;
using Beauty.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Beauty.ViewModels;

namespace Beauty.Controllers
{
    public class MastersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MastersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Master
        public IActionResult Index()
        {
            var masters = _context.Masters
                .Include(x => x.TypeService)
                .Select(model => new MasterViewModel()
                {
                    Id = model.Id,
                    Title = model.Title,
                    TypeServiceId = model.TypeServiceId,
                    TypeService = model.TypeService
                });

            return View(masters);
        }

        // GET: Master/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var master = _context.Masters.Include(m => m.TypeService).FirstOrDefault(m => m.Id == id);
            if (master == null)
            {
                return NotFound();
            }

            return View(master);
        }

        // GET: Master/Create
        public IActionResult Create()
        {
            ViewBag.TypeService = new SelectList(_context.TypeServices, "Id", "Title");
            return View();
        }

        // POST: Master/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,TypeServiceId")] Master master)
        {
            if (ModelState.IsValid)
            {
                _context.Add(master);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Убедимся, что ViewBag.TypeServices инициализирован
            ViewBag.TypeService = new SelectList(_context.TypeServices, "Id", "Title", master.TypeServiceId);
            return View(master);
        }

        // GET: Master/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var master = _context.Masters.Include(m => m.TypeService).FirstOrDefault(m => m.Id == id);
            if (master == null)
            {
                return NotFound();
            }

            ViewBag.TypeService = new SelectList(_context.TypeServices, "Id", "Title", master.TypeServiceId);
            return View(master);
        }

        // POST: Master/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,TypeServiceId")] Master master)
        {
            if (id != master.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(master);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MasterExists(master.Id))
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

            ViewBag.TypeService = new SelectList(_context.TypeServices, "Id", "Title", master.TypeServiceId);
            return View(master);
        }

        // GET: Master/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var master = _context.Masters.Include(m => m.TypeService).FirstOrDefault(m => m.Id == id);
            if (master == null)
            {
                return NotFound();
            }

            return View(master);
        }

        // POST: Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var master = _context.Masters.FirstOrDefault(m => m.Id == id);
            _context.Masters.Remove(master);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool MasterExists(int id)
        {
            return _context.Masters.Any(e => e.Id == id);
        }
    }
}