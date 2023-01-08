namespace Exchanger.Models
{
    public class Error
    {
        public string? IdRequest { get; set; }
        public bool IdRequestShow => !string.IsNullOrEmpty(IdRequest);

        public Error(string? idRequest)
        {
            IdRequest = idRequest;
        }
    }
}