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

        public string Images { get; set; } // a list of URLs separated by '|'

        public Offer(string title, string? description, string images, int? idProfile = null, int? id = null)
        {
            Title = title;
            Description = description;

            Images = images;

            if (idProfile != null)
            {
                IdProfile = (int)idProfile;
            }

            if (id != null)
            {
                Id = (int)id;
            }
        }
    }
}
