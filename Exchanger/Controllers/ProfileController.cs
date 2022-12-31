using Microsoft.AspNetCore.Mvc;

using Exchanger.Models.Offer;
using Exchanger.Models.Profile;

namespace Exchanger.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            if (false)
            {
                Test();
                return View();
            }
            else
            {
                return Redirect("~/Account/LogIn");
            }
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

            Profile profile = new()
            {
                Avatar = "image/userUnknown.png",
                City = "Craiova",
                Country = "Romania",
                Description = "Un bulangiu si juma :P... ca na csf naicsf",
                Email = "dennis69@gmail.com",
                Name = "Jnitzi",
                Phone = "0770337470",
                Rating = 3.4f
            };

            ViewData["Profile"] = profile;
        }
    }
}
