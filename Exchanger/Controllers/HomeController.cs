using Exchanger.Data;
using Exchanger.Models;
using Exchanger.Models.View;
using Exchanger.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Index(int? page, string? keyword, string? country, string? city)
        {
            var idProfile = HttpContext.Session.GetInt32("Profile.Id");
            var offersAll = _context.Offer.Where(offer => offer.IdProfile != idProfile);

            var (countries, cities) = GetOfferLocationAll(offersAll);
            ViewData["Offers.Countries"] = countries.OrderBy(country => country).ToList();
            ViewData["Offers.Cities"] = cities.OrderBy(city => city).ToList();

            Filter filter = new()
            {
                Keyword = keyword,
                Country = country,
                City = city,
            };
            ViewData["Offers.Filter"] = filter;
            offersAll = FilterOffers(offersAll, filter);

            ViewData["Offers.All"] = await ListPaginated<Offer>.CreateAsync(offersAll.OrderByDescending(o => o.Id), page ?? 1);
            ViewData["Offers.Mine"] = await _context.Offer.Where(o => o.IdProfile == idProfile).OrderByDescending(o => o.Id).ToListAsync();

            return View("Index", filter);
        }

        IQueryable<Offer> FilterOffers(IQueryable<Offer> offers, Filter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                var offersAllFilteredByKeyword1 = offers.Where(offer => offer.Title.Contains(filter.Keyword));
                var offersAllFilteredByKeyword2 = offers.Where(offer => offer.Description != null && offer.Description.Contains(filter.Keyword));

                offers = offersAllFilteredByKeyword1.Concat(offersAllFilteredByKeyword2).Distinct();
            }

            if (filter.Country != null && filter.Country != "Country")
            {
                offers = offers.Where(offer => _context.Profile.Where(profile => profile.Id == offer.IdProfile).First().Country == filter.Country);
            }

            if (filter.City != null && filter.City != "City")
            {
                offers = offers.Where(offer => filter.City != null && _context.Profile.Where(profile => profile.Id == offer.IdProfile).First().City == filter.City);
            }

            return offers;
        }

        (string country, string? city) GetOfferLocation(Offer offer)
        {
            var profile = _context.Profile.Where(profile => profile.Id == offer.IdProfile).First();
            return (profile.Country, profile.City);
        }

        (IQueryable<string> countries, IQueryable<string> cities) GetOfferLocationAll(IQueryable<Offer> offers)
        {
            HashSet<string> countries = new();
            HashSet<string> cities = new();

            foreach (var offer in offers)
            {
                var (country, city) = GetOfferLocation(offer);

                countries.Add(country);
                if (city != null)
                {
                    cities.Add(city);
                }
            }

            return (countries.AsQueryable(), cities.AsQueryable());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Filter([Bind("Country,City,Keyword")] Filter filter)
        {
            var url = "~/?";

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                url += "keyword=" + filter.Keyword + "&";
            }

            if (filter.Country != null && filter.Country != "Country")
            {
                url += "country=" + filter.Country + "&";
            }

            if (filter.City != null && filter.City != "City")
            {
                url += "city=" + filter.City + "&";
            }

            if (url.Last() == '&')
            {
                url = url.Remove(url.Length - 1);
            }

            return Redirect(url);
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