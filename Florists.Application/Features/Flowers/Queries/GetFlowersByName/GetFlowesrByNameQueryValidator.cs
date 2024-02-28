using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Flowers.Queries.GetFlowersByName
{
  public class GetFlowesrByNameQueryValidator : AbstractValidator<GetFlowersByNameQuery>
  {
    public GetFlowesrByNameQueryValidator()
    {
      RuleFor(x => x.FlowerName)
        .MaximumLength(64)
        .WithMessage(Messages.Flowers.FlowerNameMaxLengthIs + 64);

      RuleFor(x => x.Page)
        .GreaterThan(0)
        .WithMessage(Messages.Pagination.PageNumber);

      RuleFor(x => x.PerPage)
       .GreaterThan(0)
       .WithMessage(Messages.Pagination.PerPage);
    }
  }
}
