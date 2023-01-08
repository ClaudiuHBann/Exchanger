using Exchanger.Data;
using Exchanger.Models;

namespace Exchanger.Services
{
    public class OfferToOfferUtility
    {
        static public List<OfferToOfferView> GetOfferToOfferViewsYourOther(ExchangerContext context, int profileId)
        {
            List<OfferToOfferView> offerToOfferYourOther = new();

            try
            {
                var profileOffers = context.Offer.Where(offer => offer.IdProfile == profileId);
                if (profileOffers == null)
                {
                    return offerToOfferYourOther;
                }

                foreach (var offerToOffer in context.OfferToOffer)
                {
                    var profileOfferToOffers = profileOffers.Where(offer => offer.Id == offerToOffer.IdOfferOffer);
                    if (profileOfferToOffers != null && profileOfferToOffers.Any())
                    {
                        var offerTo = context.Offer.Where(offer => offer.Id == offerToOffer.IdOffer).First();
                        if (offerTo == null)
                        {
                            continue;
                        }

                        foreach (var offer in profileOfferToOffers)
                        {
                            offerToOfferYourOther.Add(new(offerToOffer.Id, offerTo, offer));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
            }

            return offerToOfferYourOther;
        }

        static public List<OfferToOfferView> GetOfferToOfferViewsOtherYour(ExchangerContext context, int profileId)
        {
            List<OfferToOfferView> offerToOfferOtherYour = new();

            try
            {
                var profileOffers = context.Offer.Where(offer => offer.IdProfile == profileId);
                if (profileOffers == null)
                {
                    return offerToOfferOtherYour;
                }

                foreach (var offerToOffer in context.OfferToOffer)
                {
                    var profileOfferToOffers = profileOffers.Where(offer => offer.Id == offerToOffer.IdOffer);
                    if (profileOfferToOffers != null && profileOfferToOffers.Any())
                    {
                        var offerTo = context.Offer.Where(offer => offer.Id == offerToOffer.IdOfferOffer).First();
                        if (offerTo == null)
                        {
                            continue;
                        }

                        foreach (var offer in profileOfferToOffers)
                        {
                            offerToOfferOtherYour.Add(new(offerToOffer.Id, offer, offerTo));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
            }

            return offerToOfferOtherYour;
        }
    }
}
