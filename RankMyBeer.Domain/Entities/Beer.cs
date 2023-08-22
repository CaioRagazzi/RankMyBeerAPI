namespace RankMyBeerDomain.Entities;
public class Beer
{
    public Beer()
    {
        this.BeerPhotos = new List<BeerPhoto>();
    }

    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string? Brand { get; set; }
    public string? Opinion { get; set; }
    public decimal? Price { get; set; }
    public required decimal Score { get; set; }
    public required string User { get; set; }
    public string? Location { get; set; }
    public ICollection<BeerPhoto> BeerPhotos { get; set; }
}
