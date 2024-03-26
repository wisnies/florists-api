using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Inventories.Queries.GetInventoriesByName
{
  public class GetInventoriesByNameQueryValidator : AbstractValidator<GetInventoriesByNameQuery>
  {
    public GetInventoriesByNameQueryValidator()
    {
      RuleFor(x => x.InventoryName)
        .MaximumLength(64)
        .WithMessage(Messages.InventoryName.MaxLengthIs + 64);

      RuleFor(x => x.Page)
        .GreaterThan(0)
        .WithMessage(Messages.Pagination.PageNumber);

      RuleFor(x => x.PerPage)
       .GreaterThan(0)
       .WithMessage(Messages.Pagination.PerPage);
    }
  }
}
