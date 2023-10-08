namespace RankMyBeerApplication.Services.UserService.Dtos;
public class UserDtoRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool IsGoogleSignIn { get; set; }
}
