using System.Linq;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Pizza_Demo.Data;

namespace Pizza_Demo.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Order newOrder)
        {
            _context.Order.Add(newOrder);
            _context.SaveChanges();
            TempData["Text"] = "Danke für Ihre Bestellung";
            //return RedirectToAction("Detail", new {Id = newOrder.Id});
            return PartialView("Detail", newOrder);
        }

        public IActionResult Detail(long id)
        {
            var order = _context.Order.FirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                return NotFound();
            }            
            TempData["Text"] = TempData["Text"] ?? "Ihre Bestellung";
            return View("Detail", order);
        }

        public IActionResult Delete(long id)
        {
            var order = _context.Order.FirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            if (order.State == OrderState.New)
            {
                order.State = OrderState.Deleted;
                _context.SaveChanges();
                return RedirectToAction("Detail", new {Id = id});
            }
            return BadRequest();
        }
    }
}