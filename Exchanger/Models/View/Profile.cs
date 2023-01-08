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
        public string? Phone { get; set; }
        public string Country { get; set; }
        public string? City { get; set; }
        public float Rating { get; set; }
    }
}