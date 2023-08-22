using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RankMyBeerDomain.Entities;

namespace RankMyBeerInfrastructure.Configurations;
public class BeerPhotoConfiguration : IEntityTypeConfiguration<BeerPhoto>
{
    public void Configure(EntityTypeBuilder<BeerPhoto> builder)
    {
        builder
            .HasOne<Beer>(s => s.Beer)
            .WithMany(g => g.BeerPhotos)
            .HasForeignKey(s => s.BeerId);

        builder
            .Ignore(e => e.UrlBucketName);

        builder
            .Ignore(e => e.PhotoURL);
    }
}
