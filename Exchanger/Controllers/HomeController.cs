using Exchanger.Data;
using Exchanger.Models;
using Exchanger.Services;
using Exchanger.Models.View;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Exchanger.Controllers
{
    public class HomeController : Controller
    {
        readonly ExchangerContext _context;

        public HomeController(ExchangerContext context)
        {
            _context = context;
        }

        IQueryable<Offer> FilterOffers(IQueryable<Offer> offers, Filter filter)
        {
            try
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
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return offers;
        }

        async Task<(string country, string? city)> GetOfferLocation(Offer offer)
        {
            try
            {
                var profile = await _context.Profile.Where(profile => profile.Id == offer.IdProfile).FirstAsync();
                return (profile.Country, profile.City);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return ("", null);
            }
        }

        async Task<(IQueryable<string>? countries, IQueryable<string>? cities)> GetOfferLocationAll(IQueryable<Offer> offers)
        {
            HashSet<string> countries = new();
            HashSet<string> cities = new();

            foreach (var offer in offers)
            {
                var (country, city) = await GetOfferLocation(offer);

                if (country != "")
                {
                    countries.Add(country);
                }

                if (!string.IsNullOrEmpty(city))
                {
                    cities.Add(city);
                }
            }

            try
            {
                return (countries.AsQueryable(), cities.AsQueryable());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return (null, null);
            }
        }

        public async Task<IActionResult> Index(int? page, string? keyword, string? country, string? city)
        {
            var idProfile = HttpContext.Session.GetInt32("Profile.Id");

            try
            {
                IQueryable<Offer> offersAll;
                if (idProfile != null)
                {
                    offersAll = _context.Offer.Where(offer => offer.IdProfile != idProfile);
                }
                else
                {
                    offersAll = _context.Offer.Select(offer => offer);
                }

                var (countries, cities) = await GetOfferLocationAll(offersAll);
                if (countries != null)
                {
                    ViewData["Offers.Countries"] = countries.OrderBy(country => country).ToList();
                }
                if (cities != null)
                {
                    ViewData["Offers.Cities"] = cities.OrderBy(city => city).ToList();
                }

                Filter filter = new(keyword, country, city);
                ViewData["Offers.Filter"] = filter;

                offersAll = FilterOffers(offersAll, filter);

                ViewData["Offers.All"] = await ListPaginated<Offer>.CreateAsync(offersAll.OrderByDescending(offer => offer.Id), page ?? 1);
                if (idProfile != null)
                {
                    ViewData["Offers.Mine"] = _context.Offer.Where(offer => offer.IdProfile == idProfile).OrderByDescending(offer => offer.Id).ToList();
                }

                return View(filter);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return View(new Filter());
            }
        }

        static string CreateQueryFromFilter(Filter filter)
        {
            string query = "";

            if (!filter.Empty)
            {
                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query += $"keyword={filter.Keyword}&";
                }

                if (filter.Country != null && filter.Country != "Country")
                {
                    query += $"country={filter.Country}&";
                }

                if (filter.City != null && filter.City != "City")
                {
                    query += $"city={filter.City}&";
                }

                try
                {
                    if (query.Last() == '&')
                    {
                        query = query.Remove(query.Length - 1);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return "";
                }
            }

            return query;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Filter([Bind("Keyword,Country,City")] Filter filter)
        {
            return Redirect($"~/?{CreateQueryFromFilter(filter)}");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Error(Activity.Current?.Id ?? HttpContext.TraceIdentifier));
        }
    }
}