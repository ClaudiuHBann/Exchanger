namespace Exchanger.Models
{
    public class OfferViewModel
    {
        public string Id { get; set; }
        public List<Item> ItemsToGive { get; set; }
        public List<Item>? ItemsToReceive { get; set; }
        public string? Description { get; set; }
    }
}
