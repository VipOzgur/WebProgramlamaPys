using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebFinalPys.Models;

namespace WebFinalPys.Controllers
{
    [Authorize]
    public class MaasPrimController : Controller
    {
        private readonly PysDbContext _context;

        public MaasPrimController()
        {
            _context = new PysDbContext();
        }

        // GET: MaasPrim
        public async Task<IActionResult> Index()
        {
            var pysDbContext = _context.MaasPrims.Include(m => m.Admin).Include(m => m.Personel);
            return View(await pysDbContext.ToListAsync());
        }

        // GET: MaasPrim/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maasPrim = await _context.MaasPrims
                .Include(m => m.Admin)
                .Include(m => m.Personel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (maasPrim == null)
            {
                return NotFound();
            }

            return View(maasPrim);
        }

        // GET: MaasPrim/Create
        public IActionResult Create()
        {
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Ad");
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Ad");
            return View();
        }

        // POST: MaasPrim/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AdminId,PersonelId,Tutar,OdemeDurumu,Tarih,Not")] MaasPrim maasPrim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(maasPrim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Ad", maasPrim.AdminId);
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Ad", maasPrim.PersonelId);
            return View(maasPrim);
        }

        // GET: MaasPrim/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maasPrim = await _context.MaasPrims.FindAsync(id);
            if (maasPrim == null)
            {
                return NotFound();
            }
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Ad", maasPrim.AdminId);
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Ad", maasPrim.PersonelId);
            return View(maasPrim);
        }

        // POST: MaasPrim/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AdminId,PersonelId,Tutar,OdemeDurumu,Tarih,Not")] MaasPrim maasPrim)
        {
            if (id != maasPrim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maasPrim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaasPrimExists(maasPrim.Id))
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
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Ad", maasPrim.AdminId);
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Ad", maasPrim.PersonelId);
            return View(maasPrim);
        }

        // GET: MaasPrim/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maasPrim = await _context.MaasPrims
                .Include(m => m.Admin)
                .Include(m => m.Personel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (maasPrim == null)
            {
                return NotFound();
            }

            return View(maasPrim);
        }

        // POST: MaasPrim/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maasPrim = await _context.MaasPrims.FindAsync(id);
            if (maasPrim != null)
            {
                _context.MaasPrims.Remove(maasPrim);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaasPrimExists(int id)
        {
            return _context.MaasPrims.Any(e => e.Id == id);
        }
    }
}
