using AutoMapper;
using RankMyBeerApplication.Services.BeerPhotoService.Dtos;
using RankMyBeerDomain.Entities;

namespace RankMyBeerApplication.Services.BeerPhotoService.MapperConfig;
public class BeerPhotoProfile : Profile
{
    public BeerPhotoProfile()
    {
        CreateMap<BeerPhoto, BeerPhotoDtoResponse>();
    }
}
