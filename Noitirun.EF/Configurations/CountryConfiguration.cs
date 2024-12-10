using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Noitirun.Core.Entities;
using NoitirunApp.Infrastructure.Data;

namespace NoitirunApp.Infrastructure.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {

            builder.ToTable("country");
            builder.HasKey(c => c.Id);
        }
    }
}
