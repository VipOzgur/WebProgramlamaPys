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
            var pysDbContext = (User.FindFirst(ClaimTypes.Role).Value != "User") ? _context.Izinlers.Include(i => i.Personel).Include(p => p.Admin) : _context.Izinlers.Include(i => i.Personel).Include(p => p.Admin).Where(x => x.PersonelId == long.Parse(User.FindFirst(ClaimTypes.Sid).Value));
            return View(await pysDbContext.OrderByDescending(m => m.Id).ToListAsync());
        }
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
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

            var izinler = (User.FindFirst(ClaimTypes.Role).Value != "User") ? await _context.Izinlers
                .Include(i => i.Personel).Include(p => p.Admin)
                .FirstOrDefaultAsync(m => m.Id == id) : await _context.Izinlers
                .Include(i => i.Personel).Include(p => p.Admin)
                .FirstOrDefaultAsync(m => m.Id == id && m.PersonelId == long.Parse(User.FindFirst(ClaimTypes.Sid).Value));

            if (izinler == null)
            {
                return NotFound();
            }

            return View(izinler);
        }

        // GET: Izinler/Create
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult Create()
        {
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Ad");
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Ad");
            return View();
        }

        // POST: Izinler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Create([FromForm] Izinler izinler)
        {
            if (ModelState.IsValid)
            {
                _context.Add(izinler);
                await _context.SaveChangesAsync();
                TempData["mesaj"] = "İzinler e eklendi";
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Ad", izinler.AdminId);
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Ad", izinler.PersonelId);
            return View(izinler);
        }

        // GET: Izinler/Edit/5
        [Authorize(Roles = "Admin, SuperAdmin")]
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
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Ad", izinler.AdminId);
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Ad", izinler.PersonelId);
            return View(izinler);
        }

        // POST: Izinler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, SuperAdmin")]
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
                    if (!IzinlerExists((int)izinler.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["mesaj"] = "İzinler Güncellendi";
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminId"] = new SelectList(_context.Personels, "Id", "Ad", izinler.AdminId);
            ViewData["PersonelId"] = new SelectList(_context.Personels, "Id", "Ad", izinler.PersonelId);
            return View(izinler);
        }

        // GET: Izinler/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin")]
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
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var izinler = await _context.Izinlers.FindAsync(id);
            if (izinler != null)
            {
                _context.Izinlers.Remove(izinler);
            }
            TempData["mesaj"] = "İzin Silindi";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IzinlerExists(int id)
        {
            return _context.Izinlers.Any(e => e.Id == id);
        }
    }
}
