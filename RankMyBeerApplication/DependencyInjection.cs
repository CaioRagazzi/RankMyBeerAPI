using Microsoft.Extensions.DependencyInjection;
using RankMyBeerApplication.BeerInterface.Interfaces;
using RankMyBeerApplication.BeerServices;
using RankMyBeerApplication.Services.User;
using RankMyBeerApplication.Services.User.Interfaces;

namespace RankMyBeerApplication;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBeerService, BeerService>();

        return services;
    }
}
