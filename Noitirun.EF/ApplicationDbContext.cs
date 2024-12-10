using Microsoft.EntityFrameworkCore;
using Noitirun.Core.Entities;
using NoitirunApp.Infrastructure.Configurations;

namespace NoitirunApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CountryConfiguration());
        }
    }
}
