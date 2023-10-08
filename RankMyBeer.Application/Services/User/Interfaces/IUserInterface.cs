using RankMyBeerApplication.Services.UserService.Dtos;

namespace RankMyBeerApplication.Services.UserInterface.Interfaces;
public interface IUserService
{
    Task Create(UserDtoRequest userDtoRequest);
}
