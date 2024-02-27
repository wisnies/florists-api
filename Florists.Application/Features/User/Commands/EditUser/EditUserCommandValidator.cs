using Florists.Core.Common.Messages;
using Florists.Core.Enums;
using FluentValidation;

namespace Florists.Application.Features.User.Commands.EditUser
{
  public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
  {
    public EditUserCommandValidator()
    {
      RuleFor(x => x.Email)
        .NotEmpty()
        .WithMessage(Messages.Auth.EmailIsRequired)
        .EmailAddress()
        .WithMessage(Messages.Auth.EmailMustBeValid);

      RuleFor(x => x.FirstName)
       .NotEmpty()
       .WithMessage(Messages.User.FirstNameIsRequired)
       .MinimumLength(2)
       .WithMessage(Messages.User.FirstNameMinLengthIs + 2)
       .MaximumLength(32)
       .WithMessage(Messages.User.FirstNameMaxLengthIs + 32);

      RuleFor(x => x.LastName)
        .NotEmpty()
        .WithMessage(Messages.User.LastNameIsRequired)
        .MinimumLength(2)
        .WithMessage(Messages.User.LastNameMinLengthIs + 2)
        .MaximumLength(32)
        .WithMessage(Messages.User.LastNameMaxLengthIs + 32);

      RuleFor(x => x.RoleType)
        .NotEmpty()
        .WithMessage(Messages.User.RoleIsRequired)
        .Must(x => IsValidRole(x))
        .WithMessage(Messages.User.InvalidUserRole)
        .Must(x => IsNotAdminRole(x))
        .WithMessage(Messages.User.UnableToCreateAdminRole);

    }
    private bool IsNotAdminRole(RoleTypeOptions roleType)
    {
      return roleType != RoleTypeOptions.Admin;
    }

    private bool IsValidRole(RoleTypeOptions roleType)
    {
      return Enum.IsDefined(typeof(RoleTypeOptions), roleType);
    }
  }
}
