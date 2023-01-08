using Microsoft.AspNetCore.Mvc;

namespace Exchanger.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
