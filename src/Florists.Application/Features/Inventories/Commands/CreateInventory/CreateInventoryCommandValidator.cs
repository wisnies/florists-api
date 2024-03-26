using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Inventories.Commands.CreateInventory
{
  public class CreateInventoryCommandValidator : AbstractValidator<CreateInventoryCommand>
  {
    public CreateInventoryCommandValidator()
    {
      RuleFor(x => x.InventoryName)
        .NotEmpty()
        .WithMessage(Messages.InventoryName.IsRequired)
        .MinimumLength(2)
        .WithMessage(Messages.InventoryName.MinLengthIs + 2)
        .MaximumLength(64)
        .WithMessage(Messages.InventoryName.MaxLengthIs + 64);

      RuleFor(x => x.AvailableQuantity)
        .NotEmpty()
        .WithMessage(Messages.Quantity.IsRequired)
        .GreaterThan(0)
        .WithMessage(Messages.Quantity.MustBeGreaterThanZero);

      RuleFor(x => x.UnitPrice)
        .NotEmpty()
        .WithMessage(Messages.UnitPrice.IsRequired)
        .GreaterThan(0)
        .WithMessage(Messages.UnitPrice.MustBeGreaterThanZero);

      RuleFor(x => x.Category)
        .NotEmpty()
        .WithMessage(Messages.Inventories.CategoryIsRequired)
        .IsInEnum()
        .WithMessage(Messages.Inventories.CategoryIsInvalid);

    }
  }
}
