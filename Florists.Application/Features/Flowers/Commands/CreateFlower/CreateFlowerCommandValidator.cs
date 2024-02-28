using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Flowers.Commands.CreateFlower
{
  public class CreateFlowerCommandValidator : AbstractValidator<CreateFlowerCommand>
  {
    public CreateFlowerCommandValidator()
    {
      RuleFor(x => x.FlowerName)
        .NotEmpty()
        .WithMessage(Messages.Flowers.FlowerNameIsRequired)
        .MinimumLength(2)
        .WithMessage(Messages.Flowers.FlowerNameMinLengthIs + 2)
        .MaximumLength(64)
        .WithMessage(Messages.Flowers.FlowerNameMaxLengthIs + 64);

      RuleFor(x => x.AvailableQuantity)
        .NotEmpty()
        .WithMessage(Messages.Flowers.AvailableQuantityIsRequired)
        .GreaterThan(0)
        .WithMessage(Messages.Flowers.AvailableQuantityMustBeGreaterThanZero);

      RuleFor(x => x.UnitPrice)
        .NotEmpty()
        .WithMessage(Messages.Flowers.UnitPriceIsRequired)
        .GreaterThan(0)
        .WithMessage(Messages.Flowers.UnitPriceMustBeGreaterThanZero);
    }
  }
}
