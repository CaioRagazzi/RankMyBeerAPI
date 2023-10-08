using Microsoft.AspNetCore.Mvc;
using RankMyBeerApplication.Services.AuthService.Dtos;
using RankMyBeerApplication.Services.AuthService.Interfaces;

namespace RankMyBeerWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Auth(AuthDtoRequest authDtoRequest)
    {
        var result = await _authService.Auth(authDtoRequest);

        if (result)
            return Ok();

        return Forbid();
    }
}
