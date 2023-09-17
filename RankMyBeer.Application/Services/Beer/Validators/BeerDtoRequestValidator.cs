using FluentValidation;
using RankMyBeerApplication.Services.BeerService.Dtos;

namespace RankMyBeerApplication.Services.BeerService.BeerValidator;
public class BeerDtoRequestValidator : AbstractValidator<BeerDtoRequest>
{
    public BeerDtoRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.User).NotEmpty();
        RuleFor(x => x.Score).NotEmpty();
    }
}
