using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using RankMyBeerApplication.Services.BeerService.Dtos;
using RankMyBeerDomain.Entities.Beer;
using RankMyBeerDomain.Models;

namespace Namespace;
public class BeerMapperConfig : Profile
{
    public BeerMapperConfig()
    {
        CreateMap<BeerDtoRequest, Beer>();
        CreateMap<Beer, BeerDtoResponse>();
        CreateMap<PagedResult<Beer>, PagedResult<BeerDtoResponse>>();
        CreateMap<JsonPatchDocument<BeerDtoRequest>, JsonPatchDocument<Beer>>();
        CreateMap<Operation<BeerDtoRequest>, Operation<Beer>>();
    }
}
