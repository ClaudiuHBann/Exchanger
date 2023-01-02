using System.ComponentModel.DataAnnotations.Schema;

namespace Exchanger.Models
{
  public class OfferToOffer
  {
        public int Id { get; set; }
        [ForeignKey("Offer")]
        public int IdOffer { get; set; }
        [ForeignKey("Offer")]
        public int IdOfferOffer { get; set; }
    }
}
