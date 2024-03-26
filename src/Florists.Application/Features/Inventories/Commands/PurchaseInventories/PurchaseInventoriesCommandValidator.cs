using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Inventories.Commands.PurchaseInventories
{
  public class PurchaseInventoriesCommandValidator : AbstractValidator<PurchaseInventoriesCommand>
  {
    public PurchaseInventoriesCommandValidator()
    {
      RuleFor(x => x.PurchaseOrderNumber)
        .NotEmpty()
        .WithMessage(Messages.OrderNumber.IsRequired)
        .MinimumLength(3)
        .WithMessage(Messages.OrderNumber.MinLengthIs + 3)
      .MaximumLength(36)
        .WithMessage(Messages.OrderNumber.MaxLengthIs + 36);

      RuleFor(x => x.Email)
        .NotEmpty()
        .WithMessage(Messages.Email.IsRequired)
        .EmailAddress()
        .WithMessage(Messages.Email.IsInvalid);

      RuleFor(x => x.InventoriesToPurchase)
        .Must(x => x.Count > 0)
        .WithMessage(Messages.Collections.LengthMustBeGreaterThanZero);

      RuleForEach(x => x.InventoriesToPurchase).ChildRules(y =>
      {
        y.RuleFor(y => y.InventoryId)
        .NotEmpty()
        .WithMessage(Messages.GuidId.IsRequired);

        y.RuleFor(y => y.QuantityToPurchase)
        .NotEmpty()
        .WithMessage(Messages.Quantity.IsRequired)
        .GreaterThan(0)
        .WithMessage(Messages.Quantity.MustBeGreaterThanZero);
      });
    }
  }
}
