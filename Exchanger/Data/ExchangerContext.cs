using Microsoft.EntityFrameworkCore;

using Exchanger.Models;

namespace Exchanger.Data
{
    public class ExchangerContext : DbContext
    {
        public ExchangerContext(DbContextOptions<ExchangerContext> options) : base(options) { }

        public DbSet<Account> Account { get; set; }
        public DbSet<Profile> Profile { get; set; }
    }
}
