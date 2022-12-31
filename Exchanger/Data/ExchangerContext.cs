using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Exchanger.Models.Profile;

namespace Exchanger.Data
{
    public class ExchangerContext : DbContext
    {
        public ExchangerContext (DbContextOptions<ExchangerContext> options)
            : base(options)
        {
        }

        public DbSet<Exchanger.Models.Profile.Account> Account { get; set; } = default!;

        public DbSet<Exchanger.Models.Profile.ProfileViewModel> ProfileViewModel { get; set; }
    }
}
