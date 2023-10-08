using AutoMapper;
using RankMyBeerApplication.Services.UserInterface.Interfaces;
using RankMyBeerApplication.Services.UserService.Dtos;
using RankMyBeerDomain.Entities;
using RankMyBeerInfrastructure.Repositories.UserRepository;

namespace RankMyBeerApplication.Services.UserService;
public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserService(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task Create(UserDtoRequest userDtoRequest)
    {
        var user = _mapper.Map<User>(userDtoRequest);

        user.Id = Guid.NewGuid();
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        await _userRepository.Insert(user);
        await _userRepository.SaveAsync();
    }
}
