using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Users.Commands.EditUser
{
  public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
  {
    public EditUserCommandValidator()
    {
      RuleFor(x => x.Email)
        .NotEmpty()
        .WithMessage(Messages.Email.IsRequired)
        .EmailAddress()
        .WithMessage(Messages.Email.IsInvalid);

      RuleFor(x => x.FirstName)
       .NotEmpty()
       .WithMessage(Messages.FirstName.IsRequired)
       .MinimumLength(2)
       .WithMessage(Messages.FirstName.MinLengthIs + 2)
       .MaximumLength(32)
       .WithMessage(Messages.FirstName.MaxLengthIs + 32);

      RuleFor(x => x.LastName)
        .NotEmpty()
        .WithMessage(Messages.LastName.IsRequired)
        .MinimumLength(2)
        .WithMessage(Messages.LastName.MinLengthIs + 2)
        .MaximumLength(32)
        .WithMessage(Messages.LastName.MaxLengthIs + 32);

      RuleFor(x => x.RoleType)
        .IsInEnum()
        .WithMessage(Messages.Users.RoleIsInvalid);

    }
  }
}
