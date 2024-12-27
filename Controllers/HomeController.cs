using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using WebFinalPys.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebFinalPys.Controllers
{
    public class HomeController : Controller
    {
        private readonly PysDbContext _logger;

        public HomeController()
        {
            _logger = new PysDbContext();
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                TempData["mesaj"] = $"Ad: {User.FindFirst(ClaimTypes.NameIdentifier).Value} Role: {User.FindFirst(ClaimTypes.Role).Value} Giris yapýldý.";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm]Personel p)
        {
            if (p.Ad == null || p.Password== null)
            {
                TempData["hata"] = "Boþluklarý Doldurunuz";
                return RedirectToAction("Login", "Home");
            }
            var helper = new HelperClass();
            var personel = _logger.Personels.Include(p => p.Role).FirstOrDefault(x => x.Ad == p.Ad && x.Password == helper.Hash(p.Password));
            if (personel != null) {
                List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier,personel.Ad),
                new Claim(ClaimTypes.UserData,(personel.Profil!= null)?personel.Profil:"/images/default.png"),
                new Claim(ClaimTypes.Role,(personel.Role!=null)?personel.Role.Ad:"User"),
                new Claim(ClaimTypes.Sid,Convert.ToString(personel.Id)),
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                TempData["mesaj"] = $"Ad: {personel.Ad} Role: {personel.Role.Ad} Giris yapýldý.";
                return RedirectToAction("Index", "Home");
            }
            TempData["hata"] = "Giriþ bilgileriniz yanlýþ";
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
