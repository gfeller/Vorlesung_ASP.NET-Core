using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Pizza_Demo.Data;
using Pizza_Demo.Models;

namespace Pizza_Demo.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController()
        {

        }

        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Order newOrder)
        {
            TempData["Text"] = "Danke für Ihre Bestellung";
            //return RedirectToAction("Detail", new {Id = newOrder.Id});
            return PartialView("Detail", newOrder);
        }

        public IActionResult Detail(long id)
        {
            TempData["Text"] = TempData["Text"] ?? "Ihre Bestellung";
            return View("Detail", new Order {Id = id, Name = "Hawaii"});
        }

        public IActionResult Delete(long id)
        {
            throw new NotImplementedException();
        }
    }
}