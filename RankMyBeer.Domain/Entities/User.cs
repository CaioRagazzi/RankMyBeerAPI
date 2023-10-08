namespace RankMyBeerDomain.Entities;
public class User
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool IsGoogleSignIn { get; set; }
}
