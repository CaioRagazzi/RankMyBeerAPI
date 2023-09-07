using Microsoft.AspNetCore.Mvc;
using RankMyBeerApplication.Services.BeerPhotoService;
using RankMyBeerApplication.Services.BeerPhotoService.Dtos;

namespace RankMyBeerWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BeerPhotoController : ControllerBase
{
    private readonly IBeerPhotoService _beerPhotoService;

    public BeerPhotoController(IBeerPhotoService beerPhotoService)
    {
        _beerPhotoService = beerPhotoService;
    }

    [HttpPost()]
    public async Task<IActionResult> AddBeerPhoto(BeerPhotoDtoRequest beerPhotoDtoRequest)
    {
        await _beerPhotoService.AddPhoto(beerPhotoDtoRequest);

        return Ok();
    }

    [HttpDelete("{beerPhotoId}")]
    public async Task<IActionResult> DeleteBeerPhoto([FromRoute] Guid beerPhotoId)
    {
        await _beerPhotoService.RemoveBeerPhoto(beerPhotoId);

        return Ok();
    }
}
