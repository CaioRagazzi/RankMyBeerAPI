using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RankMyBeerApplication.Services.BeerInterface.Interfaces;
using RankMyBeerApplication.Services.BeerPhotoService;
using RankMyBeerApplication.Services.BeerService.Dtos;
using RankMyBeerApplication.Services.BeerServices;
using RankMyBeerApplication.Services.PhotoBucketService;
using RankMyBeerApplication.Services.User;
using RankMyBeerApplication.Services.User.Interfaces;

namespace RankMyBeerApplication;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBeerService, BeerService>();
        services.AddScoped<IBeerPhotoService, BeerPhotoService>();
        services.AddScoped<IPhotoBucketService, PhotoBucketService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssemblyContaining<BeerDtoRequest>();

        return services;
    }
}
