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
    public class MesailerController : Controller
    {
        private readonly PysDbContext _context;

        public MesailerController()
        {
            _context = new PysDbContext();
        }

        // GET: Mesailer
        public async Task<IActionResult> Index()
        {
            var pysDbContext = _context.Mesailers.Include(m => m.Admin).Include(m => m.PersonelNavigation);
            return View(await pysDbContext.ToListAsync());
        }

        // GET: Mesailer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesailer = await _context.Mesailers
                .Include(m => m.Admin)
                .Include(m => m.PersonelNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mesailer == null)
            {
                return NotFound();
            }

            return View(mesailer);
        }

        // GET: Mesailer/Create
        public IActionResult Create()
        {
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Ad");
            ViewData["Personel"] = new SelectList(_context.Personels, "Id", "Ad");
            return View();
        }

        // POST: Mesailer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AdminId,Personel,StartTime,FinishTime,SaatlikUcret,Tutar,OdenmeDurumu,Not,Tarih")] Mesailer mesailer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mesailer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Ad", mesailer.AdminId);
            ViewData["Personel"] = new SelectList(_context.Personels, "Id", "Ad", mesailer.Personel);
            return View(mesailer);
        }

        // GET: Mesailer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesailer = await _context.Mesailers.FindAsync(id);
            if (mesailer == null)
            {
                return NotFound();
            }
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Ad", mesailer.AdminId);
            ViewData["Personel"] = new SelectList(_context.Personels, "Id", "Ad", mesailer.Personel);
            return View(mesailer);
        }

        // POST: Mesailer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AdminId,Personel,StartTime,FinishTime,SaatlikUcret,Tutar,OdenmeDurumu,Not,Tarih")] Mesailer mesailer)
        {
            if (id != mesailer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mesailer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MesailerExists(mesailer.Id))
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
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Ad", mesailer.AdminId);
            ViewData["Personel"] = new SelectList(_context.Personels, "Id", "Ad", mesailer.Personel);
            return View(mesailer);
        }

        // GET: Mesailer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesailer = await _context.Mesailers
                .Include(m => m.Admin)
                .Include(m => m.PersonelNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mesailer == null)
            {
                return NotFound();
            }

            return View(mesailer);
        }

        // POST: Mesailer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mesailer = await _context.Mesailers.FindAsync(id);
            if (mesailer != null)
            {
                _context.Mesailers.Remove(mesailer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MesailerExists(int id)
        {
            return _context.Mesailers.Any(e => e.Id == id);
        }
    }
}
