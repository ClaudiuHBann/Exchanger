using Exchanger.Data;
using Exchanger.Models.View;

namespace Exchanger.Models
{
    public class OfferToOfferView
    {
        public int Id { get; set; }
        public Offer? Offer { get; set; } = null;
        public Offer? OfferToOffer { get; set; } = null;

        public OfferToOfferView(int id, Offer offer, Offer offerToOffer)
        {
            Id = id;
            Offer = offer;
            OfferToOffer = offerToOffer;
        }

        public OfferToOfferView(ExchangerContext context, int id, int idOffer, int idOfferToOffer)
        {
            Id = id;

            try
            {
                Offer = context.Offer.Where(offer => offer.Id == idOffer).First();
                OfferToOffer = context.Offer.Where(offer => offer.Id == idOfferToOffer).First();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public bool Good => Offer != null && OfferToOffer != null;
    }
}
