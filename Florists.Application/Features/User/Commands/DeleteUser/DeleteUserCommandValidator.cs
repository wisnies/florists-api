using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.User.Commands.DeleteUser
{
  public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
  {
    public DeleteUserCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty().WithMessage(Messages.User.UserIdIsRequired);
    }
  }
}
