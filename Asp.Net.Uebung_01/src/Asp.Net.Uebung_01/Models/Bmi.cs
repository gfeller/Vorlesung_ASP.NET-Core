using System.ComponentModel.DataAnnotations;

namespace Asp.Net.Uebung_01.Models
{
    public class Bmi
    {
        [Display(Name = "Gewicht in kg")]
        public double Weight { get; set; }
 
        [Display(Name = "Höhe in cm")]
        public double Height { get; set; }
    }
}