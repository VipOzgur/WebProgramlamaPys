using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebFinalPys.Models;

namespace WebFinalPys.Controllers
{
    public class IzinlerController : Controller
    {
        private readonly PysDbContext _context;

        public IzinlerController()
        {
            _context = new PysDbContext();
        }

        // GET: Izinler
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pysDbContext = _context.Izinlers.Include(i => i.Admin).Include(i => i.Personel);
            return View(await pysDbContext.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Izinlers == null)
            {
                return Problem("Entity set is null.");
            }

            var izinler = from m in _context.Izinlers
                             select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                izinler = izinler.Include(i => i.Admin).Include(i => i.Personel).Where(s => s.Personel.Ad!.ToUpper().Contains(searchString.ToUpper()));
            }
            ViewData["searchString"] = searchString;
            return View(await izinler.ToListAsync());
        }
        // GET: Izinler/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izinler = await _context.Izinlers
                .Include(i => i.Admin)
                .Include(i => i.Personel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (izinler == null)
            {
                return NotFound();
            }

            return View(izinler);
        }

        // GET: Izinler/Create
        public IActionResult Create()
        {
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Id");
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Id");
            return View();
        }

        // POST: Izinler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AdminId,PersonelId,IzinTipi,StartDate,FinishDate,Not,Tarih")] Izinler izinler)
        {
            if (ModelState.IsValid)
            {
                _context.Add(izinler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Id", izinler.AdminId);
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Id", izinler.PersonelId);
            return View(izinler);
        }

        // GET: Izinler/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izinler = await _context.Izinlers.FindAsync(id);
            if (izinler == null)
            {
                return NotFound();
            }
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Id", izinler.AdminId);
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Id", izinler.PersonelId);
            return View(izinler);
        }

        // POST: Izinler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AdminId,PersonelId,IzinTipi,StartDate,FinishDate,Not,Tarih")] Izinler izinler)
        {
            if (id != izinler.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(izinler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IzinlerExists(izinler.Id))
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
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Id", izinler.AdminId);
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Id", izinler.PersonelId);
            return View(izinler);
        }

        // GET: Izinler/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izinler = await _context.Izinlers
                .Include(i => i.Admin)
                .Include(i => i.Personel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (izinler == null)
            {
                return NotFound();
            }

            return View(izinler);
        }

        // POST: Izinler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var izinler = await _context.Izinlers.FindAsync(id);
            if (izinler != null)
            {
                _context.Izinlers.Remove(izinler);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IzinlerExists(int id)
        {
            return _context.Izinlers.Any(e => e.Id == id);
        }
    }
}
