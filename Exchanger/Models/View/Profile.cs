using System.ComponentModel.DataAnnotations.Schema;

namespace Exchanger.Models.View
{
    public class Profile
    {
        public int Id { get; set; }
        [ForeignKey("Account")]
        public int IdAccount { get; set; }

        public string? Avatar { get; set; } = null;
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;

        public string Email { get; set; }
        public string? Phone { get; set; } = null;

        public string Country { get; set; }
        public string? City { get; set; } = null;

        public float Rating { get; set; }

        public Profile() { }

        public Profile(string email, string country, float rating, string? avatar = null, string? name = null, string? description = null, string? phone = null, string? city = null, int? idAccount = null, int? id = null)
        {
            Avatar = avatar;
            Name = name;
            Description = description;

            Email = email;
            Phone = phone;

            Country = country;
            City = city;

            Rating = rating;

            if (idAccount != null)
            {
                IdAccount = (int)idAccount;
            }

            if (id != null)
            {
                Id = (int)id;
            }
        }

        static public Profile CreateEmpty(Account account) => new(
            account.Email,
            "Unknown",
            1f,
            "~/image/userUnknown.png",
            "Unknown",
            "Unknown",
            "Unknown",
            "Unknown",
            account.Id
            );
    }
}