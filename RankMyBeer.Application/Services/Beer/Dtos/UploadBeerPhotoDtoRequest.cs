namespace RankMyBeerApplication.Services.BeerService.Dtos;
public class UploadBeerPhotoDtoRequest
{
    public Guid BeerId { get; set; }
    public required string Base64Photo { get; set; }
    public required string FileName { get; set; }
}
