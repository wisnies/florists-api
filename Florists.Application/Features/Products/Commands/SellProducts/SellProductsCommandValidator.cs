using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Products.Commands.SellProducts
{
  public class SellProductsCommandValidator : AbstractValidator<SellProductsCommand>
  {
    public SellProductsCommandValidator()
    {
      RuleFor(x => x.SaleOrderNumber)
        .NotEmpty()
        .WithMessage(Messages.OrderNumber.IsRequired)
        .MinimumLength(3)
        .WithMessage(Messages.OrderNumber.MinLengthIs + 3)
        .MaximumLength(16)
        .WithMessage(Messages.OrderNumber.MaxLengthIs + 16);

      RuleFor(x => x.Email)
        .NotEmpty()
        .WithMessage(Messages.Email.IsRequired)
        .EmailAddress()
        .WithMessage(Messages.Email.IsInvalid);

      RuleFor(x => x.ProductsToSell)
        .Must(x => x.Count > 0)
        .WithMessage(Messages.Collections.LengthMustBeGreaterThanZero);

      RuleForEach(x => x.ProductsToSell).ChildRules(y =>
      {
        y.RuleFor(y => y.ProductId)
        .NotEmpty()
        .WithMessage(Messages.GuidId.IsRequired);

        y.RuleFor(y => y.QuantityToSell)
        .NotEmpty()
        .WithMessage(Messages.Quantity.IsRequired)
        .GreaterThan(0)
        .WithMessage(Messages.Quantity.MustBeGreaterThanZero);
      });
    }
  }
}
