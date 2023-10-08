using Microsoft.AspNetCore.Mvc;
using RankMyBeerApplication.Services.UserInterface.Interfaces;
using RankMyBeerApplication.Services.UserService.Dtos;

namespace RankMyBeerWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(UserDtoRequest userDtoRequest)
    {
        await _userService.Create(userDtoRequest);

        return Ok();
    }
}
