using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Products.Commands.DeleteProduct
{
  public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
  {
    public DeleteProductCommandValidator()
    {
      RuleFor(x => x.ProductId)
        .NotEmpty()
        .WithMessage(Messages.GuidId.IsRequired);
    }
  }
}
