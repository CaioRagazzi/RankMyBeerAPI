using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RankMyBeerDomain.Entities.Beer;

namespace RankMyBeerInfrastructure.Configurations;
public class BeerConfiguration : IEntityTypeConfiguration<Beer>
{
    public void Configure(EntityTypeBuilder<Beer> builder)
    {
        builder
            .Property(e => e.Location)
            .HasColumnType("json");
    }
}
