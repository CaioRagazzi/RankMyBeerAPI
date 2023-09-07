namespace RankMyBeerApplication.Services.BeerPhotoService.Dtos;
public class BeerPhotoDtoRequest
{
    public Guid BeerId { get; set; }
    public required string Base64Photo { get; set; }
    public required string ImageFileName { get; set; }
}
