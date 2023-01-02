using Exchanger.Data;
using Exchanger.Models;

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Exchanger.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ExchangerContext _context;

        public HomeController(ILogger<HomeController> logger, ExchangerContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["Offers.All"] = _context.Offer.OrderByDescending(o => o.Id).ToList();

            var idProfile = HttpContext.Session.GetInt32("Profile.Id");
            ViewData["Offers.Mine"] = _context.Offer.Where(o => o.IdProfile == idProfile).OrderByDescending(o => o.Id).ToList();
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Error { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}