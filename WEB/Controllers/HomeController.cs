using Microsoft.AspNetCore.Mvc;

namespace AgroManager.PL.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
