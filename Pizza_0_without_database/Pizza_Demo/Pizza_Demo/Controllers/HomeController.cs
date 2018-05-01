using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pizza_Demo.Models;

namespace Pizza_Demo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Foo()
        {
            return Json(new {Name = "Test"});
        }

        public IActionResult Name(long id, NameSettings settings)
        {
            string name = "Pizzzzzzaaa Shop - The Best!";
            return Content(settings.LetterCase == "upper" ? name.ToUpper() : name.ToLower());
        }



    }

    public class NameSettings
    {
        public string LetterCase { get; set; } = "upper";
    }
}
