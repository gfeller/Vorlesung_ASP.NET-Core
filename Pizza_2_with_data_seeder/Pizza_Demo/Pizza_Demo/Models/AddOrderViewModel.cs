using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizza_Demo.Models
{
    public class NewOrderViewModel
    {
        [Display(Name = "Pizza Name")]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
