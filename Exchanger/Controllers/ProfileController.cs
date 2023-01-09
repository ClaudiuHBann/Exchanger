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

        public IActionResult Index()
        {
            return View();
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            await PopulateProfile(account);
            return View("Details");
        }

        public async Task PopulateProfile(Account account)
        {
            var profile = await _context.Profile.FirstOrDefaultAsync(p => p.IdAccount == account.Id);
            if (profile == null)
            {
                return;
            }

            ViewData["Profile"] = profile;
            ViewData["Offers"] = await _context.Offer.Where(o => o.IdProfile == profile.Id).OrderByDescending(o => o.Id).ToListAsync();
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit()
        {
            var idProfile = HttpContext.Session.GetInt32("Profile.Id");
            if (idProfile == null || _context.Profile == null)
            {
                return NotFound();
            }

            var profile = await _context.Profile.Where(profile => profile.Id == idProfile).FirstAsync();
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            var profileOld = _context.Profile.Where(p => p.Id == idProfile).First();
            profile.Rating = profileOld.Rating;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(profileOld).CurrentValues.SetValues(profile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return await Details(idAccount);
            }
            return await Details(idAccount);
        }
    }
}
