using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Products.Queries.GetProductsByName
{
  public class GetProductsByNameQueryValidator : AbstractValidator<GetProductsByNameQuery>
  {
    public GetProductsByNameQueryValidator()
    {
      RuleFor(x => x.ProductName)
        .MaximumLength(64)
        .WithMessage(Messages.ProductName.MaxLengthIs + 64);

      RuleFor(x => x.Page)
        .GreaterThan(0)
        .WithMessage(Messages.Pagination.PageNumber);

      RuleFor(x => x.PerPage)
       .GreaterThan(0)
       .WithMessage(Messages.Pagination.PerPage);
    }
  }
}
