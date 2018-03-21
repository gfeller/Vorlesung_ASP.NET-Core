using System;
using System.ComponentModel.DataAnnotations;

namespace Uebung.Models
{
    public class Bmi
    {
        [Range(0, 300)]
        [Display(Name = "Gewicht in kg")]
        public double Weight { get; set; }

        [Range(30, 250)]
        [Display(Name = "Höhe in cm")]
        public double Height { get; set; }
    }
}