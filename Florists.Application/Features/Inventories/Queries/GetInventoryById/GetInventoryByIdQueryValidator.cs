using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Inventories.Queries.GetInventoryById
{
  public class GetInventoryByIdQueryValidator : AbstractValidator<GetInventoryByIdQuery>
  {
    public GetInventoryByIdQueryValidator()
    {
      RuleFor(x => x.InventoryId)
        .NotEmpty()
        .WithMessage(Messages.GuidId.IsRequired);
    }
  }
}
