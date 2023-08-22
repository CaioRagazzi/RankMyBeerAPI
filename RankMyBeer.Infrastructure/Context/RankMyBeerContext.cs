using Microsoft.EntityFrameworkCore;
using RankMyBeerDomain.Entities;
using RankMyBeerInfrastructure.Configurations;

namespace RankMyBeerInfrastructure.Context;
public class RankMyBeerContext : DbContext
{
    public DbSet<Beer> Beers { get; set; }

    public RankMyBeerContext()
    {

    }

    public RankMyBeerContext(DbContextOptions<RankMyBeerContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        new BeerConfiguration().Configure(builder.Entity<Beer>());
        new BeerPhotoConfiguration().Configure(builder.Entity<BeerPhoto>());
    }
}