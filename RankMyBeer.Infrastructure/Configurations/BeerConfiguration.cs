using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RankMyBeerDomain.Entities;

namespace RankMyBeerInfrastructure.Configurations;
public class BeerConfiguration : IEntityTypeConfiguration<Beer>
{
    public void Configure(EntityTypeBuilder<Beer> builder)
    {
        builder
            .Property(e => e.Location)
            .HasColumnType("json");

        builder
            .HasMany(s => s.BeerPhotos)
            .WithOne(s => s.Beer)
            .HasForeignKey(s => s.BeerId);
    }
}
