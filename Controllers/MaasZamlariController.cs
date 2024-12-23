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
    public class MaasZamlariController : Controller
    {
        private readonly PysDbContext _context;

        public MaasZamlariController()
        {
            _context = new PysDbContext();
        }

        // GET: MaasZamlari
        public async Task<IActionResult> Index()
        {
            var pysDbContext = _context.MaasZamlaris.Include(m => m.Admin).Include(m => m.Personel);
            return View(await pysDbContext.ToListAsync());
        }

        // GET: MaasZamlari/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maasZamlari = await _context.MaasZamlaris
                .Include(m => m.Admin)
                .Include(m => m.Personel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (maasZamlari == null)
            {
                return NotFound();
            }

            return View(maasZamlari);
        }

        // GET: MaasZamlari/Create
        public IActionResult Create()
        {
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Id");
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Id");
            return View();
        }

        // POST: MaasZamlari/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AdminId,PersonelId,Tutar,Tarih,Aciklama,Yuzde,Tip")] MaasZamlari maasZamlari)
        {
            if (ModelState.IsValid)
            {
                _context.Add(maasZamlari);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Id", maasZamlari.AdminId);
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Id", maasZamlari.PersonelId);
            return View(maasZamlari);
        }

        // GET: MaasZamlari/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maasZamlari = await _context.MaasZamlaris.FindAsync(id);
            if (maasZamlari == null)
            {
                return NotFound();
            }
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Id", maasZamlari.AdminId);
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Id", maasZamlari.PersonelId);
            return View(maasZamlari);
        }

        // POST: MaasZamlari/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AdminId,PersonelId,Tutar,Tarih,Aciklama,Yuzde,Tip")] MaasZamlari maasZamlari)
        {
            if (id != maasZamlari.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maasZamlari);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaasZamlariExists(maasZamlari.Id))
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
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Id", maasZamlari.AdminId);
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Id", maasZamlari.PersonelId);
            return View(maasZamlari);
        }

        // GET: MaasZamlari/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maasZamlari = await _context.MaasZamlaris
                .Include(m => m.Admin)
                .Include(m => m.Personel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (maasZamlari == null)
            {
                return NotFound();
            }

            return View(maasZamlari);
        }

        // POST: MaasZamlari/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maasZamlari = await _context.MaasZamlaris.FindAsync(id);
            if (maasZamlari != null)
            {
                _context.MaasZamlaris.Remove(maasZamlari);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaasZamlariExists(int id)
        {
            return _context.MaasZamlaris.Any(e => e.Id == id);
        }
    }
}
