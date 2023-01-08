using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Exchanger.Data;

using System.Data;
using System.Text;
using Exchanger.Models.View;

namespace Exchanger.Controllers
{
    public class AccountController : Controller
    {
        private readonly ExchangerContext _context;

        public AccountController(ExchangerContext context)
        {
            _context = context;
        }

        // GET: Account
        [HttpGet]
        [Route("Account/LogIn")]
        public async Task<IActionResult> Index()
        {
            var accountActive = HttpContext.Session.GetInt32("Account.Active");
            if (accountActive != null && accountActive == 1)
            {
                return await Details(HttpContext.Session.GetInt32("Account.Id"));
            }
            else
            {
                return View("Index");
            }
        }

        // POST: Account/LogIn
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn([Bind("Email,Password")] Account account)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Account.AnyAsync(e => e.Email == account.Email && e.Password == account.Password))
                {
                    HttpContext.Session.SetInt32("Account.Active", 1);
                    var idAcc = _context.Account.Where(a => a.Email == account.Email && a.Password == account.Password).First().Id;
                    HttpContext.Session.SetInt32("Account.Id", idAcc);
                    HttpContext.Session.SetString("Account.Email", account.Email);
                    HttpContext.Session.SetString("Account.Password", account.Password);

                    HttpContext.Session.SetInt32("Profile.Id", _context.Profile.Where(p => p.IdAccount == idAcc).First().Id);
                }

                return RedirectToAction("Index");
            }
            return View(account);
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
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

        // GET: Account/SignUp
        public IActionResult SignUp()
        {
            return View("Create");
        }

        // POST: Account/SignUp
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([Bind("Email,Password")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Account.Add(account);
                await _context.SaveChangesAsync();

                var idAcc = _context.Account.Where(acc => account.Email == acc.Email && account.Password == acc.Password).First().Id;
                var profile = Profile.CreateEmpty(new(account.Email, account.Password, idAcc));
                _context.Profile.Add(profile);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(account);
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
                    /*if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }*/
                }
                return await Details(idAccount);
            }
            return await Details(idAccount);
        }

        /*// GET: Account/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(p => p.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Account == null)
            {
                return Problem("Entity set 'ExchangerContext.Account'  is null.");
            }
            var account = await _context.Account.FindAsync(id);
            if (account != null)
            {
                _context.Account.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.Id == id);
        }*/
    }
}
