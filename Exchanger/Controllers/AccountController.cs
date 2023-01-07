using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Exchanger.Data;
using Exchanger.Models;

using System.Data;

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
            await LogIn(new Account() { Email = "a@a.a", Password = "a@a.a" });

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

        public async Task<IActionResult> LogOut()
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
                Profile profile = new()
                {
                    Avatar = "image/userUnknown.png",
                    City = "Unknown",
                    Country = "Unknown",
                    Description = "None",
                    Email = account.Email,
                    IdAccount = idAcc,
                    Name = "Unknown",
                    Phone = "Unknown",
                    Rating = 1f
                };
                _context.Profile.Add(profile);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(account);
        }

        /*// GET: Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Account/Delete/5
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
