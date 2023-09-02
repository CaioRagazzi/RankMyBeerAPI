using RankMyBeerApplication.Services.BeerPhotoService.Dtos;

namespace RankMyBeerApplication.Services.BeerService.Dtos;
public class BeerDtoResponse
{
    public BeerDtoResponse()
    {
        BeerPhotos = new List<BeerPhotoDtoResponse>();
    }
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string? Brand { get; set; }
    public string? Opinion { get; set; }
    public decimal? Price { get; set; }
    public required decimal Score { get; set; }
    public required string User { get; set; }
    public string? Location { get; set; }
    public IEnumerable<BeerPhotoDtoResponse> BeerPhotos { get; set; }
}
