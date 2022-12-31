using Microsoft.EntityFrameworkCore;

using Exchanger.Models.Profile;

namespace Exchanger.Data
{
    public class ExchangerContext : DbContext
    {
        public ExchangerContext(DbContextOptions<ExchangerContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; } = default!;
        public DbSet<Profile> Profiles { get; set; }
    }
}
