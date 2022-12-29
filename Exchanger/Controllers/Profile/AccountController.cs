using Microsoft.AspNetCore.Mvc;

namespace Exchanger.Controllers.Profile
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View("LogIn");
        }

        public IActionResult LogIn()
        {
            return View("LogIn");
        }

        public IActionResult SignUp()
        {
            return View("SignUp");
        }
    }
}
