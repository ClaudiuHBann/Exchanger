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
                                "Image11.png",
                                "Image12.png",
                                "Image13.png"
                            }
                        },
                        new()
                        {
                            Id = 2,
                            Name = "Item2",
                            Images = new()
                            {
                                "Image21.png",
                                "Image22.png",
                                "Image23.png"
                            }
                        },
                        new()
                        {
                            Id = 3,
                            Name = "Item3",
                            Images = new()
                            {
                                "Image31.png",
                                "Image32.png",
                                "Image33.png"
                            }
                        }
                    },
                    Description = "ceva123"
                }
            };
            offers.First().ItemsToReceive = offers.First().ItemsToGive;

            for (int i = 0; i < 1000; i++)
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