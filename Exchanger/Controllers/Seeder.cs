using Microsoft.EntityFrameworkCore;

using Exchanger.Data;
using Exchanger.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Exchanger.Controllers
{
    public class Seeder
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ExchangerContext(serviceProvider.GetRequiredService<DbContextOptions<ExchangerContext>>());

            // clear all
            foreach (var account in context.Account)
            {
                context.Account.Remove(account);
            }
            foreach (var profile in context.Profile)
            {
                context.Profile.Remove(profile);
            }
            foreach (var offer in context.Offer)
            {
                context.Offer.Remove(offer);
            }
            await context.SaveChangesAsync();

            // add all
            var accounts = await GetListOfTFromURL<Account>("https://my.api.mockaroo.com/users.json?key=d7ccea10");
            if (accounts == null)
            {
                return;
            }
            await context.Account.AddRangeAsync(accounts);

            var profiles = await GetListOfTFromURL<Profile>("https://my.api.mockaroo.com/profile.json?key=d7ccea10");
            if (profiles == null)
            {
                return;
            }
            await context.SaveChangesAsync();
            var accountsUpdated = await context.Account.ToListAsync();
            for (var i = 0; i < accountsUpdated.Count; i++)
            {
                profiles[i].IdAccount = accountsUpdated[i].Id;
                profiles[i].Avatar = "image/userUnknown.png";
                profiles[i].Email = accounts[i].Email;
            }
            await context.Profile.AddRangeAsync(profiles);

            var offers = await GetListOfTFromURL<Offer>("https://my.api.mockaroo.com/offer.json?key=d7ccea10");
            if (offers == null)
            {
                return;
            }
            await context.SaveChangesAsync();
            var profilesUpdated = await context.Profile.ToListAsync();
            for (var i = 0; i < profilesUpdated.Count; i++)
            {
                offers[i].IdProfile = profilesUpdated[i].Id;
                string images = "";
                for (var j = 0; j < int.Parse(offers[i].Images); j++)
                {
                    images += "image/itemUnknown.png|";
                }
                offers[i].Images = images.Remove(images.Length - 1);
            }
            await context.Offer.AddRangeAsync(offers);

            await context.SaveChangesAsync();

            // add special
            Account acc = new()
            {
                Email = "a@a.a",
                Password = "a@a.a"
            };
            await context.Account.AddAsync(acc);
            await context.SaveChangesAsync();

            var idAcc = context.Account.Where(account => account.Email == acc.Email && account.Password == acc.Password).First().Id;
            Profile prof = new()
            {
                Avatar = "https://scontent.fcra1-1.fna.fbcdn.net/v/t39.30808-6/288147638_1697156760663806_412457527889072342_n.jpg?_nc_cat=100&ccb=1-7&_nc_sid=09cbfe&_nc_ohc=dkfHvdTknxcAX8ZXTNF&tn=p_LORh6lWoQDHZxv&_nc_ht=scontent.fcra1-1.fna&oh=00_AfAtkp6Y2ia8VmxMXqN6RE7t2tarVELJAQW7d2jKpMLXPg&oe=63B4F332",
                City = "Craiova",
                Country = "Romania",
                Description = "Nu risti, nu pierzi.",
                Email = "claudiu.hermann@caphyon.com",
                IdAccount = idAcc,
                Name = "Claudiu HBann",
                Phone = "0770337470",
                Rating = 5f
            };
            await context.Profile.AddAsync(prof);
            await context.SaveChangesAsync();

            Offer off = new()
            {
                IdProfile = context.Profile.Where(profile => profile.IdAccount == idAcc).First().Id,
                Title = "Vand scula",
                Description = "Pe bascula",
                Images = "https://i0.wp.com/www.orgasmbox.co.uk/wp-content/uploads/2021/07/Dildo.jpg|https://media1.lajumate.ro/media/i/api_list/8/140/14090618_bascula-iveco-bremach_3.jpg"
            };
            await context.Offer.AddAsync(off);
            await context.SaveChangesAsync();
        }

        static readonly HttpClient httpClient = new();
        static async Task<string> URLToString(string url)
        {
            return await httpClient.GetStringAsync(url);
        }

        static async Task<List<T>?> GetListOfTFromURL<T>(string url)
        {
            var json = await URLToString(url);
            var jArray = JsonConvert.DeserializeObject<JArray>(json);
            return jArray?.ToObject<List<T>>();
        }
    }
}
