namespace Exchanger.Models.View
{
    public class Account
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public Account() { }

        public Account(string email, string password, int? id = null)
        {
            Email = email;
            Password = password;

            if (id != null)
            {
                Id = (int)id;
            }
        }
    }
}
