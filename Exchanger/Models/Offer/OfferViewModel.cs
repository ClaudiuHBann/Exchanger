namespace Exchanger.Models.Offer
{
    public class OfferViewModel
    {
        public int Id { get; set; }
        public List<Item> ItemsToGive { get; set; }
        public List<Item>? ItemsToReceive { get; set; } = null;
        public string? Description { get; set; } = null;
    }
}
