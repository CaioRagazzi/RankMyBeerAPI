using Microsoft.AspNetCore.Mvc;
using RankMyBeerApplication.BeerInterface.Interfaces;
using RankMyBeerDomain.Entities.Beer;

namespace RankMyBeerWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BeerController : ControllerBase
{
    private readonly IBeerService _beerService;

    public BeerController(IBeerService beerService)
    {
        _beerService = beerService;
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var beer = await _beerService.GetBeer(id);

        return Ok(beer);
    }

    [HttpGet("user-id/{userId}")]
    public async Task<IActionResult> Get(string userId, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var beer = await _beerService.GetBeer(userId, page, pageSize);

        return Ok(beer);
    }

    [HttpPost]
    public async Task<IActionResult> AddBeer(Beer beer)
    {
        await _beerService.AddBeer(beer);

        return Ok();
    }
}
