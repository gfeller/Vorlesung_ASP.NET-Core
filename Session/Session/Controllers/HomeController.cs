using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Session.Controllers
{
    public class HomeController : Controller
    {
        private const string SesssionKey = "Key";
        public IActionResult Index()
        {
            var value = (HttpContext.Session.GetInt32(SesssionKey) ?? 0) + 1;
            HttpContext.Session.SetInt32(SesssionKey, value);
            return Content($"Dein {value}-xter Besuch");
        }
    }
}
