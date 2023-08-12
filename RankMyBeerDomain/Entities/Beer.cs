namespace RankMyBeerDomain.Entities.Beer;
public class Beer
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string? Brand { get; set; }
    public string? Opinion { get; set; }
    public decimal? Price { get; set; }
    public required decimal Score { get; set; }
    public required string User { get; set; }
    // public object? Location { get; set; }
    public string? PhotoURL { get; set; }
}
