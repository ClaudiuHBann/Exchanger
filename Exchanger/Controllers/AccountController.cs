using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Exchanger.Data;
using Exchanger.Models.View;

using System.Data;

namespace Exchanger.Controllers
{
    public class AccountController : Controller
    {
        readonly ExchangerContext _context;

        public AccountController(ExchangerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Account/")]
        [Route("Account/Login")]
        public IActionResult Index()
        {
            var accountActive = HttpContext.Session.GetInt32("Account.Active");
            var profileId = HttpContext.Session.GetInt32("Account.Id");
            if (accountActive != null && accountActive == 1 && profileId != null)
            {
                return Redirect($"~/Profile/Details/{profileId}");
            }
            else
            {
                return View("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] Account account)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _context.Account.AnyAsync(a => a.Email == account.Email && a.Password == account.Password))
                    {
                        HttpContext.Session.SetInt32("Account.Active", 1);

                        var acc = await _context.Account.Where(a => a.Email == account.Email && a.Password == account.Password).FirstAsync();

                        HttpContext.Session.SetInt32("Account.Id", acc.Id);
                        HttpContext.Session.SetString("Account.Email", account.Email);
                        HttpContext.Session.SetString("Account.Password", account.Password);

                        var profile = await _context.Profile.Where(p => p.IdAccount == acc.Id).FirstAsync();
                        HttpContext.Session.SetInt32("Profile.Id", profile.Id);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return LogOut();
                }
            }

            return Index();
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return Index();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([Bind("Email,Password")] Account account)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Account.AddAsync(account);
                    await _context.SaveChangesAsync();

                    var acc = await _context.Account.Where(acc => account.Email == acc.Email && account.Password == acc.Password).FirstAsync();

                    var profile = Profile.CreateEmpty(new(account.Email, account.Password, acc.Id));
                    await _context.Profile.AddAsync(profile);
                    await _context.SaveChangesAsync();

                    return Index();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return SignUp();
                }
            }

            return View(account);
        }
    }
}