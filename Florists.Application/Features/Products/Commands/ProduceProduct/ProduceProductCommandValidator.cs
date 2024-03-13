using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Products.Commands.ProduceProduct
{
  public class ProduceProductCommandValidator : AbstractValidator<ProduceProductCommand>
  {
    public ProduceProductCommandValidator()
    {
      RuleFor(x => x.ProductId)
        .NotEmpty()
        .WithMessage(Messages.GuidId.IsRequired);

      RuleFor(x => x.QuantityToProduce)
        .NotEmpty()
        .WithMessage(Messages.Quantity.IsRequired)
        .GreaterThan(0)
        .WithMessage(Messages.Quantity.MustBeGreaterThanZero);

      RuleFor(x => x.ProductionOrderNumber)
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
    }
  }
}
