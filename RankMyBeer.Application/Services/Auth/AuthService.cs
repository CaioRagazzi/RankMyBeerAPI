using AutoMapper;
using RankMyBeerApplication.Services.AuthService.Dtos;
using RankMyBeerApplication.Services.AuthService.Interfaces;
using RankMyBeerInfrastructure.Repositories.UserRepository;

namespace RankMyBeerApplication.Services.AuthService;
public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public AuthService(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<bool> Auth(AuthDtoRequest authDtoRequest)
    {
        var user = await _userRepository.Get(1, 1, x => x.Email == authDtoRequest.Email);

        if (user.Results.Any())
        {
            var isValid = BCrypt.Net.BCrypt.EnhancedVerify(authDtoRequest.Password, user.Results.FirstOrDefault()?.Password);

            if (isValid)
                return true;
        }

        return false;
    }
}
