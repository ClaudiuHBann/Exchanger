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

            /*foreach (var account in context.Account)
            {
                context.Account.Remove(account);
            }*/
            /*foreach (var profile in context.Profile)
            {
                context.Profile.Remove(profile);
            }*/
            context.SaveChanges();

            Account acc = new()
            {
                Email = "a@a.a",
                Password = "a@a.a"
            };
            /*context.Account.Add(acc);*/
            context.SaveChanges();

            /*Profile prof = new()
            {
                Avatar = "userUnknown.png",
                City = "Craiova",
                Country = "Romania",
                Description = "...",
                Email = "a@a.a",
                IdAccount = context.Account.Where(e => e.Email == acc.Email && e.Password == acc.Password).First().Id,
                Name = "User#6996",
                Phone = "0770337470",
                Rating = 5f
            };*/
            /*context.Profile.Add(prof);*/
            context.SaveChanges();
        }
    }
}
