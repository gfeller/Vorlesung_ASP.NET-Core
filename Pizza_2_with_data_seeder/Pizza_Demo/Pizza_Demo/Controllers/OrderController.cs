﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizza_Demo.Data;
using Pizza_Demo.Models;
using Pizza_Demo.Utilities;

namespace Pizza_Demo.Controllers
{
	[Authorize]
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
        public IActionResult Create(NewOrderViewModel newOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var order = new Order() {Name = newOrder.Name, Date = DateTime.Now, CustomerId = User.GetId()}; //or _userManager.GetUserId(User);
            _context.Order.Add(order);
            _context.SaveChanges();
            TempData["Text"] = "Danke für Ihre Bestellung";
            return PartialView("Detail", order);
        }

        public IActionResult Detail(long id)
        {
            var order = _context.Order.FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            if (!User.IsAdmin() && order.CustomerId != User.GetId())
            {
                return Forbid();
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

            if (!User.IsAdmin() && order.CustomerId != User.GetId())
            {
                return Forbid();
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