using Exchanger.Data;

namespace Exchanger.Models
{
    public class OfferToOfferUtility
    {
        static public List<OfferToOfferView> GetOfferToOfferViewsYourOther(ExchangerContext context, int profileId)
        {
            var profileOffers = context.Offer.Where(offer => offer.IdProfile == profileId);

            List<OfferToOfferView> offersYourOther = new();
            foreach (var offerToOffer in context.OfferToOffer)
            {
                var profileOffersForOffer = profileOffers.Where(offer => offer.Id == offerToOffer.IdOfferOffer);
                if (profileOffersForOffer.Any())
                {
                    var offerOut = context.Offer.Where(offer => offer.Id == offerToOffer.IdOffer).First();
                    foreach (var offer in profileOffersForOffer)
                    {
                        offersYourOther.Add(new()
                        {
                            Id = offerToOffer.Id,
                            Offer = offerOut,
                            OfferOffer = offer
                        });
                    }
                }
            }

            return offersYourOther;
        }

        static public List<OfferToOfferView> GetOfferToOfferViewsOtherYour(ExchangerContext context, int profileId)
        {
            var profileOffers = context.Offer.Where(offer => offer.IdProfile == profileId);

            List<OfferToOfferView> offersOtherYour = new();
            foreach (var offerToOffer in context.OfferToOffer)
            {
                var profileOffersTo = profileOffers.Where(offer => offer.Id == offerToOffer.IdOffer);
                if (profileOffersTo.Any())
                {
                    var offerOut = context.Offer.Where(offer => offer.Id == offerToOffer.IdOfferOffer).First();
                    foreach (var offer in profileOffersTo)
                    {
                        offersOtherYour.Add(new()
                        {
                            Id = offerToOffer.Id,
                            Offer = offer,
                            OfferOffer = offerOut
                        });
                    }
                }
            }

            return offersOtherYour;
        }
    }
}
