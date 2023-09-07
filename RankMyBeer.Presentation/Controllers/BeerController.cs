using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RankMyBeerApplication.Services.BeerInterface.Interfaces;
using RankMyBeerApplication.Services.BeerService.Dtos;

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
    public async Task<IActionResult> Get(string userId, [FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var beer = await _beerService.GetBeer(userId, page, pageSize);

        return Ok(beer);
    }

    [HttpPost]
    public async Task<IActionResult> AddBeer(BeerDtoRequest beerDtoRequest)
    {
        var beerId = await _beerService.AddBeer(beerDtoRequest);

        return Ok(beerId);
    }

    [HttpPatch("{id:Guid}")]
    public async Task<IActionResult> PatchBeer([FromRoute] Guid id, [FromBody] JsonPatchDocument<BeerDtoRequest> patchDocument)
    {
        await _beerService.PartialUpdate(id, patchDocument);

        return Ok();
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateBeer([FromRoute] Guid id, [FromBody] BeerDtoRequest patchDocument)
    {
        await _beerService.Update(id, patchDocument);

        return Ok();
    }

    [HttpDelete("{beerId}")]
    public async Task<IActionResult> Delete([FromRoute] Guid beerId)
    {
        await _beerService.Delete(beerId);

        return Ok();
    }
}
