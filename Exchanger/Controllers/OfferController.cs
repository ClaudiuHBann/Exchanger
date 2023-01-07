using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Exchanger.Data;
using Exchanger.Models;

namespace Exchanger.Controllers
{
    public class OfferController : Controller
    {
        private readonly ExchangerContext _context;

        public OfferController(ExchangerContext context)
        {
            _context = context;
        }

        /*// GET: Offer
        public async Task<IActionResult> Index()
        {
              return View(await _context.Offer.ToListAsync());
        }

        // GET: Offer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Offer == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }*/

        // GET: Offer/Create
        public IActionResult Create()
        {
            return View();
        }

        [Route("Offer/{id}/{idOffer}")]
        public async Task<IActionResult> Offer(int? id, int? idOffer)
        {
            if (id != null && idOffer != null)
            {
                await _context.OfferToOffer.AddAsync(new OfferToOffer()
                {
                    IdOffer = (int)id,
                    IdOfferOffer = (int)idOffer
                });
                await _context.SaveChangesAsync();
            }

            return Redirect("~/");
        }

        public IActionResult YourOther()
        {
            var profileId = (int)HttpContext.Session.GetInt32("Profile.Id");
            ViewData["Offers.YO"] = OfferToOfferUtility.GetOfferToOfferViewsYourOther(_context, profileId);

            return View("~/Views/OfferOffer/YourOther.cshtml");
        }

        public IActionResult OtherYour()
        {
            var profileId = (int)HttpContext.Session.GetInt32("Profile.Id");
            ViewData["Offers.OY"] = OfferToOfferUtility.GetOfferToOfferViewsOtherYour(_context, profileId);

            return View("~/Views/OfferOffer/OtherYour.cshtml");
        }

        // POST: Offer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Images")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                offer.IdProfile = (int)HttpContext.Session.GetInt32("Profile.Id");
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return Redirect("~/Account");
            }
            return View(offer);
        }

        // GET: Offer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Offer == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            return View(offer);
        }

        // POST: Offer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdProfile,Title,Description,Images")] Offer offer)
        {
            if (id != offer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.Id))
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
            return View(offer);
        }

        // GET: Offer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Offer == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // POST: Offer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Offer == null)
            {
                return Problem("Entity set 'ExchangerContext.Offer'  is null.");
            }
            var offer = await _context.Offer.FindAsync(id);
            if (offer != null)
            {
                _context.Offer.Remove(offer);
            }

            await _context.SaveChangesAsync();
            return Redirect("~/Account");
        }

        private bool OfferExists(int id)
        {
            return _context.Offer.Any(e => e.Id == id);
        }
    }
}
