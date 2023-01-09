using Microsoft.EntityFrameworkCore;

using Exchanger.Data;
using Exchanger.Models.View;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Exchanger.Services
{
    public class Seeder
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            try
            {
                using var context = new ExchangerContext(serviceProvider.GetRequiredService<DbContextOptions<ExchangerContext>>());

                if (context.Account.Any())
                {
                    return;
                }

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
                foreach (var offerToOffer in context.OfferToOffer)
                {
                    context.OfferToOffer.Remove(offerToOffer);
                }
                await context.SaveChangesAsync();

                // add all
                var mockarooAPIKey = "faafb590";

                var accounts = await GetListOfTFromURL<Account>($"https://my.api.mockaroo.com/users.json?key={mockarooAPIKey}");
                if (accounts == null)
                {
                    return;
                }
                await context.Account.AddRangeAsync(accounts);

                var profiles = await GetListOfTFromURL<Profile>($"https://my.api.mockaroo.com/profile.json?key={mockarooAPIKey}");
                if (profiles == null)
                {
                    return;
                }
                await context.SaveChangesAsync();
                var accountsUpdated = await context.Account.ToListAsync();
                for (var i = 0; i < accountsUpdated.Count; i++)
                {
                    profiles[i].IdAccount = accountsUpdated[i].Id;
                    profiles[i].Avatar = "../image/userUnknown.png";
                    profiles[i].Email = accounts[i].Email;
                }
                await context.Profile.AddRangeAsync(profiles);

                for (var k = 0; k < 3; k++)
                {
                    var offers = await GetListOfTFromURL<Offer>($"https://my.api.mockaroo.com/offer.json?key={mockarooAPIKey}");
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
                            images += "../image/itemUnknown.png|";
                        }

                        if (images.Length > 0)
                        {
                            images = images.Remove(images.Length - 1);
                        }
                        offers[i].Images = images;
                    }
                    await context.Offer.AddRangeAsync(offers);
                    await context.SaveChangesAsync();
                }

                // add special
                Account acc = new("a@a.a", "a@a.a");
                await context.Account.AddAsync(acc);
                await context.SaveChangesAsync();
                var accUpdated = await context.Account.Where(account => account.Email == acc.Email && account.Password == acc.Password).FirstAsync();

                Profile prof = new(
                    acc.Email,
                    "Romania",
                    5f,
                    "../image/userUnknown.png",
                    "Claudiu HBann",
                    "Nu risti, nu pierzi.",
                    "0770337470",
                    "Craiova",
                    accUpdated.Id
                    );
                await context.Profile.AddAsync(prof);
                await context.SaveChangesAsync();
                var profUpdated = await context.Profile.Where(profile => profile.IdAccount == accUpdated.Id).FirstAsync();

                await context.Offer.AddAsync(new(
                    "Vand scula",
                    "Pe bascula",
                    "https://i0.wp.com/www.orgasmbox.co.uk/wp-content/uploads/2021/07/Dildo.jpg|https://media1.lajumate.ro/media/i/api_list/8/140/14090618_bascula-iveco-bremach_3.jpg",
                    profUpdated.Id
                    ));
                await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
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
