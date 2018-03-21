﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Pizza_Demo.Models
{
    public enum OrderState
    {
        [Display(Name = "Neu")]
        New,
        InProgress,
        Shipped,
        Deleted
    }

    public class Order
    {

        public long Id { get; set; }

        [Display(Name="Pizza-Name")]
        [StringLength(20, MinimumLength = 4)]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public OrderState State { get; set; }

        public virtual ApplicationUser Customer { get; set; }
        [Required]
        public string CustomerId { get; set; }

        public Order()
        {
            Date = DateTime.Now;
        }
    }
}
