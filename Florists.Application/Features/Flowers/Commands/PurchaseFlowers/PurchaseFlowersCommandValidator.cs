using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Flowers.Commands.PurchaseFlowers
{
  public class PurchaseFlowersCommandValidator : AbstractValidator<PurchaseFlowersCommand>
  {
    public PurchaseFlowersCommandValidator()
    {
      RuleFor(x => x.PurchaseOrderNumber)
        .NotEmpty()
        .WithMessage(Messages.FlowerTransactions.PurchaseOrderNumberIsRequired);

      RuleFor(x => x.Email)
        .NotEmpty()
        .WithMessage(Messages.Auth.EmailIsRequired)
        .EmailAddress()
        .WithMessage(Messages.Auth.EmailMustBeValid);

      RuleFor(x => x.FlowersToPurchase)
        .Must(x => x.Count > 0)
        .WithMessage(Messages.FlowerTransactions.FlowersToPurchaseLengthMustBeGreaterThanZero);

      RuleForEach(x => x.FlowersToPurchase).ChildRules(y =>
      {
        y.RuleFor(y => y.FlowerId)
        .NotEmpty()
        .WithMessage(Messages.FlowerTransactions.FlowerIdIsRequired);

        y.RuleFor(y => y.QuantityToPurchase)
        .NotEmpty()
        .WithMessage(Messages.FlowerTransactions.QuantityToPurchaseIsRequired)
        .GreaterThan(0)
        .WithMessage(Messages.FlowerTransactions.QuantityToPurchaseMustBeGreaterThanZero);
      });
    }
  }
}
