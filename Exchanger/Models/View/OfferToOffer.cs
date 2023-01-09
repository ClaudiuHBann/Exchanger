using System.ComponentModel.DataAnnotations.Schema;

namespace Exchanger.Models.View
{
    public class OfferToOffer
    {
        public int Id { get; set; }
        [ForeignKey("Offer")]
        public int IdOffer { get; set; }
        [ForeignKey("Offer")]
        public int IdOfferOffer { get; set; }

        public OfferToOffer() { }

        public OfferToOffer(int idOffer, int idOfferOffer, int? id = null)
        {
            IdOffer = idOffer;
            IdOfferOffer = idOfferOffer;

            if (id != null)
            {
                Id = (int)id;
            }
        }
    }
}
