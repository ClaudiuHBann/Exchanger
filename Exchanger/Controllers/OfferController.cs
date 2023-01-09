using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Exchanger.Data;
using Exchanger.Models.View;

namespace Exchanger.Controllers
{
    public class OfferController : Controller
    {
        private readonly ExchangerContext _context;

        public OfferController(ExchangerContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [Route("Offer/{id:int}/{idOffer:int}")]
        public async Task<IActionResult> Offer(int? id, int? idOffer)
        {
            if (id != null && idOffer != null)
            {
                try
                {
                    await _context.OfferToOffer.AddAsync(new OfferToOffer((int)id, (int)idOffer));
                    await _context.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }

            return Redirect("~/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Images")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                var profileId = HttpContext.Session.GetInt32("Profile.Id");
                if (profileId == null)
                {
                    return NotFound();
                }

                offer.IdProfile = (int)profileId;

                try
                {
                    await _context.AddAsync(offer);
                    await _context.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }

                return Redirect("~/Account");
            }

            return Redirect("~/Profile");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Offer == null)
            {
                return NotFound();
            }

            try
            {
                var offer = await _context.Offer.FindAsync(id);
                if (offer == null)
                {
                    return NotFound();
                }

                return View(offer);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return NotFound();
            }
        }

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
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }

            return Redirect("~/Profile");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Offer == null)
            {
                return NotFound();
            }

            try
            {
                var offer = await _context.Offer.FirstAsync(m => m.Id == id);
                if (offer == null)
                {
                    return NotFound();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return Redirect("~/Profile");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Offer == null)
            {
                return Problem("Entity set 'ExchangerContext.Offer' is null.");
            }

            try
            {
                var offer = await _context.Offer.FindAsync(id);
                if (offer != null)
                {
                    _context.Offer.Remove(offer);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return Redirect("~/Account");
        }
    }
}
