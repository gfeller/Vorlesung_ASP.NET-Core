using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Net.Uebung_01.Models;
using Asp.Net.Uebung_01.Services;
using Microsoft.AspNetCore.Mvc;

namespace Asp.Net.Uebung_01.Controllers
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
            if (data.Weight > 0 && data.Weight < 300 
                && data.Height > 30 && data.Height < 250)
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
