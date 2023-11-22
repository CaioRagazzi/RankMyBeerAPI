using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RankMyBeerApplication.Services.AuthService;
using RankMyBeerApplication.Services.AuthService.Interfaces;
using RankMyBeerApplication.Services.BeerInterface.Interfaces;
using RankMyBeerApplication.Services.BeerPhotoService;
using RankMyBeerApplication.Services.BeerService.Dtos;
using RankMyBeerApplication.Services.BeerServices;
using RankMyBeerApplication.Services.PhotoBucketService;
using RankMyBeerApplication.Services.UserInterface.Interfaces;
using RankMyBeerApplication.Services.UserService;
namespace RankMyBeerApplication;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBeerService, BeerService>();
        services.AddScoped<IBeerPhotoService, BeerPhotoService>();
        services.AddScoped<IPhotoBucketService, PhotoBucketService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssemblyContaining<BeerDtoRequest>();

        return services;
    }
}
