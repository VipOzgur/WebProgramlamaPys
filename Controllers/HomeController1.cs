﻿using Microsoft.AspNetCore.Mvc;

namespace WebFinalPys.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
