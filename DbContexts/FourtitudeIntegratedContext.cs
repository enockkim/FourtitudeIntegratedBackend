using FourtitudeIntegrated.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FourtitudeIntegrated.DbContexts
{
    public class FourtitudeIntegratedContext : DbContext
    {
        public DbSet<Accounts> Accounts { set; get; }
        public DbSet<Documents> Documents { set; get; }
        public DbSet<DocumentTypes> DocumentTypes { set; get; }
        public DbSet<GeneralLedger> GeneralLedger { set; get; }
        public DbSet<Transactions> Transactions { set; get; }
        public DbSet<Contributions> Contributions { set; get; }
        public DbSet<AccountCategories> AccountCategories { set; get; }
        public DbSet<AccountTypes> AccountTypes { get; set; } = default!;
        public FourtitudeIntegratedContext(DbContextOptions<FourtitudeIntegratedContext> options) : base(options)
        {
        }
    }
}
