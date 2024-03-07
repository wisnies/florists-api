using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Users.Queries.GetUserById
{
  public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
  {
    public GetUserByIdQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty().WithMessage(Messages.GuidId.IsRequired);
    }
  }
}
