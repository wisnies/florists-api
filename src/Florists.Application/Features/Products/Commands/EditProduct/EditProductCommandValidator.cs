using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Products.Commands.EditProduct
{
  public class EditProductCommandValidator : AbstractValidator<EditProductCommand>
  {
    public EditProductCommandValidator()
    {
      RuleFor(x => x.ProductId)
        .NotEmpty()
        .WithMessage(Messages.GuidId.IsRequired);

      RuleFor(x => x.ProductName)
        .NotEmpty()
        .WithMessage(Messages.ProductName.IsRequired)
        .MinimumLength(2)
        .WithMessage(Messages.ProductName.MinLengthIs + 2)
        .MaximumLength(64)
        .WithMessage(Messages.ProductName.MaxLengthIs + 64);

      RuleFor(x => x.UnitPrice)
        .NotEmpty()
        .WithMessage(Messages.UnitPrice.IsRequired)
        .GreaterThan(0)
        .WithMessage(Messages.UnitPrice.MustBeGreaterThanZero);

      RuleFor(x => x.Sku)
        .NotEmpty()
        .WithMessage(Messages.Sku.IsRequired)
        .MinimumLength(2)
        .WithMessage(Messages.Sku.MinLengthIs + 2)
        .MaximumLength(16)
        .WithMessage(Messages.Sku.MaxLengthIs + 16);

      RuleFor(x => x.Category)
        .NotEmpty()
        .WithMessage(Messages.Products.CategoryIsRequired)
        .IsInEnum()
        .WithMessage(Messages.Products.CategoryIsInvalid);

      RuleFor(x => x.RequiredInventories)
        .Must(x => x.Count > 0)
        .WithMessage(Messages.Collections.LengthMustBeGreaterThanZero);

      RuleForEach(x => x.RequiredInventories)
        .ChildRules(y =>
        {
          y.RuleFor(y => y.InventoryId)
          .NotEmpty()
          .WithMessage(Messages.GuidId.IsRequired);

          y.RuleFor(y => y.RequiredQuantity)
          .NotEmpty()
          .WithMessage(Messages.Quantity.IsRequired)
          .GreaterThan(0)
          .WithMessage(Messages.Quantity.MustBeGreaterThanZero);
        });
    }
  }
}
