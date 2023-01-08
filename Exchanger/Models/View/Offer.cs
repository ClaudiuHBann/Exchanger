using System.ComponentModel.DataAnnotations.Schema;

namespace Exchanger.Models.View
{
    public class Offer
    {
        public int Id { get; set; }
        [ForeignKey("Profile")]
        public int IdProfile { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; } = null;
        public string Images { get; set; } // a list of strings separated by '|'
    }
}
