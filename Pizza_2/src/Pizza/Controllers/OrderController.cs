using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pizza.Data;
using Pizza.Models;
using Pizza.Models.OrderViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Pizza.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Create()
        {   
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(NewOrderViewModel newOrder)
        {
            if (ModelState.IsValid)
            {
                var order = new Order() { Name = newOrder.Name,CustomerId = _userManager.GetUserId(User)};
                _context.Orders.Add(order);
                _context.SaveChanges();

                TempData["Text"] = "Danke für ihre Bestellung:";
                return PartialView("Add", order);
            }
            else
            {
                return BadRequest();
            }
        }

    
        public IActionResult Show(long id)
        {
            var userId = _userManager.GetUserId(User);
            var order = _context.Orders.Where(x => x.Id == id && userId == x.CustomerId).Include(x=>x.Customer).FirstOrDefault();
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.Text = "Ihre Bestellung:";
            return View("Add", order);
        }
        
        public IActionResult Delete(long id)
        {
            var order = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            if (order.State == OrderState.New)
            {
                order.State = OrderState.Deleted;
                _context.SaveChanges();
                return View("Add", order);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}