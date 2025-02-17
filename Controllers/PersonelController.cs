﻿using System;
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
    public class PersonelController : Controller
    {
        private readonly PysDbContext _context;

        public PersonelController()
        {
            _context = new PysDbContext();
        }

        // GET: Personel
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            var pysDbContext = _context.Personels.Include(p => p.Dep).Include(p => p.Role);
            return View(await pysDbContext.ToListAsync());
        }
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Personels == null)
            {
                return Problem("Entity set is null.");
            }

            var personel = from m in _context.Personels
                             select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                personel = personel.Where(s => s.Ad!.ToUpper().Contains(searchString.ToUpper()));
            }
            ViewData["searchString"] = searchString;
            return View(await personel.ToListAsync());
        }

        // GET: Personel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if(User.FindFirst(ClaimTypes.Role).Value == "User" && (User.FindFirst(ClaimTypes.Sid).Value) != id.ToString())
            {
                return NotFound();
            }

            var personel = await _context.Personels
                .Include(p => p.Dep)
                .Include(p => p.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personel == null)
            {
                return NotFound();
            }

            return View(personel);
        }

        // GET: Personel/Create
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult Create()
        {
            ViewData["DepId"] = new SelectList(_context.Departments, "Id", "Ad");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Ad");
            return View();
        }

        // POST: Personel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Create([FromForm] Personel personel)
        {

            if (ModelState.IsValid)
            {
                if (personel.ImageFile != null)
                {
                    //personel.Profil dolu ise ekliyoruz
                    string imageExtension = Path.GetExtension(personel.ImageFile.FileName);
                    string imageName = personel.Ad + personel.Soyad + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + imageExtension;
                    string imagePath = Path.Combine("../WebFinalPys/wwwroot/images/" + imageName);
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await personel.ImageFile.CopyToAsync(fileStream);
                    }
                    personel.Profil = "/images/" + imageName; //Resim yolunu veri tabanına ekleme
                }
                else
                {
                    //değil ise varsayılanı ekliyoruz
                    personel.Profil = "/images/default.png";
                }
                if (personel.RoleId == null)
                {
                    personel.RoleId = 2;
                }
                var helper = new HelperClass();
                personel.Password =helper.Hash(personel.TcNo);//varsayılan şifreyi tc olarak ayarlıyoruz
                _context.Add(personel);
                await _context.SaveChangesAsync();
                TempData["mesaj"] = "Personel eklendi";
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepId"] = new SelectList(_context.Departments, "Id", "Ad", personel.DepId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Ad", personel.RoleId);
            return View(personel);
        }

        // GET: Personel/Edit/5
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personel = await _context.Personels.FindAsync(id);
            if (personel == null)
            {
                return NotFound();
            }
            ViewData["DepId"] = new SelectList(_context.Departments, "Id", "Ad", personel.DepId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Ad", personel.RoleId);
            return View(personel);
        }

        // POST: Personel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] 
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Edit(int id, [FromForm] Personel personel)
        {
            if (id != personel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonelExists((int)personel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["mesaj"] = "Personel Düzenlendi";
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepId"] = new SelectList(_context.Departments, "Id", "Ad", personel.DepId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Ad", personel.RoleId);
            return View(personel);
        }

        // GET: Personel/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personel = await _context.Personels
                .Include(p => p.Dep)
                .Include(p => p.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personel == null)
            {
                return NotFound();
            }

            return View(personel);
        }

        // POST: Personel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personel = await _context.Personels.FindAsync(id);
            if (personel != null)
            {
                _context.Personels.Remove(personel);
            }

            await _context.SaveChangesAsync();
            TempData["mesaj"] = "Personel Silindi";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Password()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Password([FromForm] Personel personel)
        {
            if (personel.Password == null || personel.NewPassword == null) { return NotFound(); }
            if (personel.Password.ToString() != personel.NewPassword.ToString())
            {
                ViewData["mesaj"] = "Sifre tekrari ayni olmali";
                return View();
            }
            Personel personel2 = _context.Personels.FirstOrDefault(x => x.Id == long.Parse(User.FindFirst(ClaimTypes.Sid).Value));
            if (personel2 == null)
            {
                return NotFound();
            }
            var helper = new HelperClass();
            personel2.Password = helper.Hash(personel.Password);
            _context.Update(personel2);
            _context.SaveChangesAsync();
            ViewData["mesaj"] = "Sifre guncelleme basarili";
            return View();
        }

        private bool PersonelExists(int id)
        {
            return _context.Personels.Any(e => e.Id == id);
        }
    }
}
