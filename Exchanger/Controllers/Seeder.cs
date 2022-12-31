using Microsoft.EntityFrameworkCore;

using Exchanger.Data;
using Exchanger.Models;

namespace Exchanger.Controllers
{
    public class Seeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ExchangerContext(serviceProvider.GetRequiredService<DbContextOptions<ExchangerContext>>());

            foreach (var account in context.Account)
            {
                context.Account.Remove(account);
            }
            foreach (var profile in context.Profile)
            {
                context.Profile.Remove(profile);
            }
            context.SaveChanges();

            Account acc = new()
            {
                Email = "a@a.a",
                Password = "a@a.a"
            };
            context.Account.Add(acc);
            context.SaveChanges();

            var idAcc = context.Account.Where(e => e.Email == acc.Email && e.Password == acc.Password).First().Id;
            Profile prof = new()
            {
                Avatar = "image/userUnknown.png",
                City = "Craiova",
                Country = "Romania",
                Description = "...",
                Email = "a@a.a",
                IdAccount = idAcc,
                Name = "User#6996",
                Phone = "0770337470",
                Rating = 5f
            };
            context.Profile.Add(prof);
            context.SaveChanges();

            Offer offer = new()
            {
                IdProfile = context.Profile.Where(e => e.IdAccount == idAcc).First().Id,
                Title = "Vand penis",
                Description = "Pe pasarica",
                Images = "https://i0.wp.com/www.orgasmbox.co.uk/wp-content/uploads/2021/07/Dildo.jpg|https://mindcraftstories.ro/applications/webp-express/webp-images/doc-root/images/2020/07/Mindcraftstories_P%C4%83s%C4%83ri-urbane-cine-sunt-%C8%99i-unde-le-po%C8%9Bi-vedea_Vrabia-de-cas%C4%83_Andrei-Chi%C8%99.jpg.webp"
            };
            context.Offer.Add(offer);
            context.SaveChanges();
        }
    }
}
