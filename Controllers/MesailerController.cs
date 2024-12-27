using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            return _context.Mesailers != null ? (User.FindFirst(ClaimTypes.Role).Value != "User") ?
                           View(await _context.Mesailers.Include(i => i.PersonelNavigation).Include(p => p.Admin).OrderByDescending(m => m.Id).ToListAsync()) :
                           View(await _context.Mesailers.Include(i => i.PersonelNavigation).Include(p => p.Admin).Where(p => p.Personel == long.Parse(User.FindFirst(ClaimTypes.Sid).Value)).OrderByDescending(m => m.Id).ToListAsync()) :
                           Problem("Entity set 'PysDbContext.Mesailers'  is null.");
        }

        // GET: Mesailer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesailer = (User.FindFirst(ClaimTypes.Role).Value != "User") ? await _context.Mesailers.Include(i => i.PersonelNavigation).Include(p => p.Admin)
                .FirstOrDefaultAsync(m => m.Id == id) : await _context.Mesailers.Include(i => i.PersonelNavigation).Include(p => p.Admin)
                .FirstOrDefaultAsync(m => m.Id == id && m.Personel == long.Parse(User.FindFirst(ClaimTypes.Sid).Value));
            if (mesailer == null)
            {
                return NotFound();
            }

            return View(mesailer);
        }

        // GET: Mesailer/Create
        [Authorize(Roles = "Admin, SuperAdmin")]
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
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Create([FromForm] Mesailer mesailer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mesailer);
                await _context.SaveChangesAsync();
                TempData["mesaj"] = "Mesai Eklendi";
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Ad", mesailer.AdminId);
            ViewData["Personel"] = new SelectList(_context.Personels, "Id", "Ad", mesailer.Personel);
            return View(mesailer);
        }

        // GET: Mesailer/Edit/5
        [Authorize(Roles = "Admin, SuperAdmin")]
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
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Edit(int id, [FromForm] Mesailer mesailer)
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
                    if (!MesailerExists((int)mesailer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["mesaj"] = "Mesai düzenlendi";
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Ad", mesailer.AdminId);
            ViewData["Personel"] = new SelectList(_context.Personels, "Id", "Ad", mesailer.Personel);
            return View(mesailer);
        }

        // GET: Mesailer/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin")]
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
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mesailer = await _context.Mesailers.FindAsync(id);
            if (mesailer != null)
            {
                _context.Mesailers.Remove(mesailer);
            }
            await _context.SaveChangesAsync();
            TempData["mesaj"] = "Mesai silindi";
            return RedirectToAction(nameof(Index));
        }

        private bool MesailerExists(int id)
        {
            return _context.Mesailers.Any(e => e.Id == id);
        }
    }
}
