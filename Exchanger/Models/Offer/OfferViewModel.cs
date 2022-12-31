using System.ComponentModel.DataAnnotations.Schema;

namespace Exchanger.Models.Offer
{
    public class OfferViewModel
    {
        public int Id { get; set; }
        [ForeignKey("Profile")]
        public int IdProfile { get; set; }
        public List<Item> ItemsToGive { get; set; }
        public List<Item>? ItemsToReceive { get; set; } = null;
        public string? Description { get; set; } = null;
    }
}
