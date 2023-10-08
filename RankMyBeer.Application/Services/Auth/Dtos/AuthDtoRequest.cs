namespace RankMyBeerApplication.Services.AuthService.Dtos;
public class AuthDtoRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
