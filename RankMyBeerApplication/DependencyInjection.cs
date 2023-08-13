using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RankMyBeerApplication.Services.BeerInterface.Interfaces;
using RankMyBeerApplication.Services.BeerServices;
using RankMyBeerApplication.Services.User;
using RankMyBeerApplication.Services.User.Interfaces;

namespace RankMyBeerApplication;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBeerService, BeerService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
