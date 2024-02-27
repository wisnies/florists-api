using Florists.Application.Interfaces.Services;
using Florists.Core.Common.Messages;
using Florists.Core.Enums;
using FluentValidation;

namespace Florists.Application.Features.User.Commands.CreateUser
{
  public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
  {
    private readonly IPasswordService _passwordService;
    public CreateUserCommandValidator(IPasswordService passwordService)
    {
      _passwordService = passwordService;

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

      RuleFor(x => x.Password)
        .NotEmpty()
        .WithMessage(Messages.Auth.PasswordIsRequired)
        .MinimumLength(8)
        .WithMessage(Messages.Auth.PasswordMinLengthIs + 8)
        .MaximumLength(32)
        .WithMessage(Messages.Auth.PasswordMaxLengthIs + 32)
        .Must(x => _passwordService.IsLowercase(x))
        .WithMessage(Messages.Auth.PasswordMustHaveLowercase)
        .Must(x => _passwordService.IsUppercase(x))
        .WithMessage(Messages.Auth.PasswordMustHaveUppercase)
        .Must(x => _passwordService.IsDigit(x))
        .WithMessage(Messages.Auth.PasswordMustHaveDigit)
        .Must(x => _passwordService.IsSpecial(x))
        .WithMessage(Messages.Auth.PasswordMustHaveSpecial)
        .Equal(x => x.ConfirmPassword)
        .WithMessage(Messages.Auth.PasswordsMustMatch);

      RuleFor(x => x.ConfirmPassword)
        .NotEmpty()
        .WithMessage(Messages.Auth.ConfirmPasswordIsRequired);

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
