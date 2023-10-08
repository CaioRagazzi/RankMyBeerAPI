using RankMyBeerApplication.Services.AuthService.Dtos;

namespace RankMyBeerApplication.Services.AuthService.Interfaces;
public interface IAuthService
{
    Task<bool> Auth(AuthDtoRequest authDtoRequest);
}
