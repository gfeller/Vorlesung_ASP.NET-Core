using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Foo()
        {
            return Json(new {Name = "1233"});
        }

        public class NameSettings
        {
            public string LetterCase { get; set; } = "upper";
   
        }

        public IActionResult Name(long id, NameSettings letterCase)
        {
            string name = "Pizzzzzzaaa Shop - The Best!";
            return Content(letterCase.LetterCase == "upper" ? name.ToUpper() : name.ToLower());
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
            return View();
        }
    }
}
