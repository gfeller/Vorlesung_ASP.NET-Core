using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Net.Uebung_01.Controllers;
using Asp.Net.Uebung_01.Models;

namespace Asp.Net.Uebung_01.Services
{
    public interface IBmiService
    {
        double Calculcate(Bmi data);
    }

    public class BmiService : IBmiService
    {
        public double Calculcate(Bmi data)
        {
            return Math.Round(data.Weight/Math.Pow((data.Height/100), 2),2);
        }
    }
}
