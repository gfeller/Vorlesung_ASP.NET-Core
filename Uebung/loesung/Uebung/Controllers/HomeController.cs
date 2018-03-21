using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Uebung.Models;
using Uebung.Services;

namespace Uebung.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBmiService _bmiService;

        public HomeController(IBmiService bmiService)
        {
            _bmiService = bmiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Bmi()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Bmi(Bmi data)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Value = _bmiService.Calculcate(data);
                return PartialView("Bmi");
            }
            return Content("Komische Daten");
        }


        public IActionResult Bmi2(Bmi data)
        {
            ViewBag.Value = _bmiService.Calculcate(data);
            return View("Bmi");
        }
    }
}
