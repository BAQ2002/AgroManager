using Microsoft.AspNetCore.Mvc;

namespace AgroManager.WEB.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Aqui futuramente você pode integrar com autenticação real
            if (email == "admin@agro.com" && password == "123")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuário ou senha inválidos";
            return View();
        }
    }
}
