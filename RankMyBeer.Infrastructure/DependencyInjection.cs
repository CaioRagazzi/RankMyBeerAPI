using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RankMyBeerInfrastructure.Context;
using RankMyBeerInfrastructure.Repositories.BeerPhotoRepository;
using RankMyBeerInfrastructure.Repositories.BeerRepository;

namespace RankMyBeerInfrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RankMyBeerContext>(
            options => options.UseMySql(
                configuration.GetConnectionString("MySql"),
                new MySqlServerVersion(new Version(5, 7, 20)),
                b =>
                {
                    b.MigrationsAssembly(typeof(RankMyBeerContext).Assembly.FullName);
                    b.EnableRetryOnFailure();
                }
            ), ServiceLifetime.Transient);

        services.AddScoped<IBeerRepository, BeerRepository>();
        services.AddScoped<IBeerPhotoRepository, BeerPhotoRepository>();

        return services;
    }
}
