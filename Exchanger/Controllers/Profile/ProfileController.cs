using Microsoft.AspNetCore.Mvc;

using Exchanger.Models.Offer;
using Exchanger.Models.Profile;

namespace Exchanger.Controllers.Profile
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            if (true)
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

            for (int i = 0; i < 3; i++)
            {
                offers.Add(offers.First());
            }

            ViewData["Offers"] = offers;

            ProfileViewModel profile = new()
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
