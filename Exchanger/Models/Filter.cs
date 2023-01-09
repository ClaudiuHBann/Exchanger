namespace Exchanger.Models
{
    public class Filter
    {
        public string? Keyword { get; set; } = null;
        public string? Country { get; set; } = null;
        public string? City { get; set; } = null;

        public Filter() { }

        public Filter(string? keyword = null, string? country = null, string? city = null)
        {
            Keyword = keyword;
            Country = country;
            City = city;
        }

        public bool Empty => Keyword == null && Country == null && City == null;
    }
}