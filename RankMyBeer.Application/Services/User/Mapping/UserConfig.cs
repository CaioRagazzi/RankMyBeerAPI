using AutoMapper;
using RankMyBeerApplication.Services.UserService.Dtos;
using RankMyBeerDomain.Entities;

namespace RankMyBeerApplication.Services.UserService.MapperConfig;
public class UserConfig : Profile
{
    public UserConfig()
    {
        CreateMap<UserDtoRequest, User>().ReverseMap();
    }
}
