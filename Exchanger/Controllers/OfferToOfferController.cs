using Exchanger.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exchanger.Controllers
{
    public class OfferToOfferController : Controller
    {
        readonly ExchangerContext _context;

        public OfferToOfferController(ExchangerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Exchange(int? id)
        {
            if (id == null || _context.OfferToOffer == null)
            {
                return NotFound();
            }

            var offerToOffer = await _context.OfferToOffer.FirstAsync(oToO => oToO.Id == id);
            if (offerToOffer == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer.FirstAsync(o => o.Id == offerToOffer.IdOffer);
            if (offer == null)
            {
                return NotFound();
            }

            var offerOffer = await _context.Offer.FirstAsync(o => o.Id == offerToOffer.IdOfferOffer);
            if (offerOffer == null)
            {
                return NotFound();
            }

            var offerToOffer1 = _context.OfferToOffer.Where(oToO => oToO.IdOffer == offer.Id);
            var offerToOffer2 = _context.OfferToOffer.Where(oToO => oToO.IdOfferOffer == offer.Id);
            var offerToOffer3 = _context.OfferToOffer.Where(oToO => oToO.IdOffer == offerOffer.Id);
            var offerToOffer4 = _context.OfferToOffer.Where(oToO => oToO.IdOfferOffer == offerOffer.Id);

            _context.OfferToOffer.RemoveRange(offerToOffer1);
            _context.OfferToOffer.RemoveRange(offerToOffer2);
            _context.OfferToOffer.RemoveRange(offerToOffer3);
            _context.OfferToOffer.RemoveRange(offerToOffer4);

            _context.Offer.Remove(offer);
            _context.Offer.Remove(offerOffer);

            await _context.SaveChangesAsync();

            return RedirectBack();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OfferToOffer == null)
            {
                return NotFound();
            }

            var offerToOffer = await _context.OfferToOffer.FirstAsync(oToO => oToO.Id == id);
            if (offerToOffer == null)
            {
                return NotFound();
            }

            _context.OfferToOffer.Remove(offerToOffer);
            await _context.SaveChangesAsync();

            return RedirectBack();
        }

        public IActionResult RedirectBack()
        {
            return Redirect(Request.GetTypedHeaders().Referer.ToString());
        }
    }
}
