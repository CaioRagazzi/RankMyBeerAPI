using Microsoft.EntityFrameworkCore;
using RankMyBeerDomain.Entities.Beer;

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
        // new BeerEntityTypeConfiguration().Configure(builder.Entity<Beer>());
    }
}