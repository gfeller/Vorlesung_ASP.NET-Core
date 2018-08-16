using System;
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
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public OrderState State { get; set; }
        public Order()
        {
            Date = DateTime.Now;
        }
    }
}
