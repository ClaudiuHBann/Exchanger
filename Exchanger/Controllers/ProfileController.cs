using Exchanger.Data;
using Exchanger.Models.View;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exchanger.Controllers
{
    public class ProfileController : Controller
    {
        readonly ExchangerContext _context;

        public ProfileController(ExchangerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return await Details(HttpContext.Session.GetInt32("Account.Id"));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            try
            {
                var account = await _context.Account.FirstAsync(m => m.Id == id);
                if (account == null)
                {
                    return NotFound();
                }

                await PopulateProfile(account);
                return View("Details");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return NotFound();
            }
        }

        public async Task PopulateProfile(Account account)
        {
            try
            {
                var profile = await _context.Profile.FirstAsync(p => p.IdAccount == account.Id);
                if (profile == null)
                {
                    return;
                }

                ViewData["Profile"] = profile;
                ViewData["Offers"] = await _context.Offer.Where(o => o.IdProfile == profile.Id).OrderByDescending(o => o.Id).ToListAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public async Task<IActionResult> Edit()
        {
            var idProfile = HttpContext.Session.GetInt32("Profile.Id");
            if (idProfile == null || _context.Profile == null)
            {
                return NotFound();
            }

            try
            {
                var profile = await _context.Profile.Where(profile => profile.Id == idProfile).FirstAsync();
                if (profile == null)
                {
                    return NotFound();
                }

                return View(profile);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Avatar,Name,Description,Email,Phone,Country,City,Rating")] Profile profile)
        {
            var idProfile = HttpContext.Session.GetInt32("Profile.Id");
            var idAccount = HttpContext.Session.GetInt32("Account.Id");
            if (idProfile == null || idAccount == null)
            {
                return NotFound();
            }

            profile.Id = (int)idProfile;
            profile.IdAccount = (int)idAccount;
            try
            {
                var profileOld = await _context.Profile.Where(p => p.Id == idProfile).FirstAsync();
                profile.Rating = profileOld.Rating;

                if (ModelState.IsValid)
                {
                    _context.Entry(profileOld).CurrentValues.SetValues(profile);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return await Details(idAccount);
        }
    }
}
