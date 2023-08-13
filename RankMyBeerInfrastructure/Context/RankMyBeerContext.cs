using Microsoft.EntityFrameworkCore;
using RankMyBeerDomain.Entities.Beer;
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
        // this.Database.EnsureCreated();
        // this.Database.MigrateAsync();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        new BeerConfiguration().Configure(builder.Entity<Beer>());
    }
}