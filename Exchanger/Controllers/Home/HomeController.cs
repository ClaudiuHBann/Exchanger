using Exchanger.Models;
using Exchanger.Models.Offer;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Exchanger.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Test();
            return View();
        }

        void Test()
        {
            List<OfferViewModel> offers = new()
            {
                new()
                {
                    Id = 1,
                    ItemsToGive = new()
                    {
                        new()
                        {
                            Id = 1,
                            Name = "Item1",
                            Images = new()
                            {
                                "image/itemUnknown.png",
                                "image/itemUnknown.png",
                                "image/itemUnknown.png"
                            }
                        },
                        new()
                        {
                            Id = 2,
                            Name = "Item2",
                            Images = new()
                            {
                                "image/itemUnknown.png",
                                "image/itemUnknown.png",
                                "image/itemUnknown.png"
                            }
                        },
                        new()
                        {
                            Id = 3,
                            Name = "Item3",
                            Images = new()
                            {
                                "image/itemUnknown.png",
                                "image/itemUnknown.png",
                                "image/itemUnknown.png"
                            }
                        }
                    },
                    Description = "ceva123"
                }
            };
            offers.First().ItemsToReceive = offers.First().ItemsToGive;

            for (int i = 0; i < 69; i++)
            {
                offers.Add(offers.First());
            }

            ViewData["Offers"] = offers;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}