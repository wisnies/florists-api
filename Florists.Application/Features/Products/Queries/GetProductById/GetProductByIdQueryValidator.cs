using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Products.Queries.GetProductById
{
  public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
  {
    public GetProductByIdQueryValidator()
    {
      RuleFor(x => x.ProductId)
        .NotEmpty()
        .WithMessage(Messages.GuidId.IsRequired);
    }
  }
}
