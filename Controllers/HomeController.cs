using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;

namespace MiniToDo.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("GoToLogin")]
        public IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Account");
        }
        [HttpPost("GoToRegister")]
        public IActionResult RedirectToRegister()
        {
            return RedirectToAction("Register", "Account");
        }
    }
}
