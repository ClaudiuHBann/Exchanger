using Microsoft.EntityFrameworkCore;

using Exchanger.Models;

namespace Exchanger.Data
{
    public class ExchangerContext : DbContext
    {
        public ExchangerContext(DbContextOptions<ExchangerContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<Profile>().ToTable("Profile");
        }

        public DbSet<Account> Account { get; set; } = default!;
        public DbSet<Profile> Profile { get; set; }
    }
}
