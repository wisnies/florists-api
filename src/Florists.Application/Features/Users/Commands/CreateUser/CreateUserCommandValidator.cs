using Florists.Application.Interfaces.Services;
using Florists.Core.Common.Messages;
using Florists.Core.Enums;
using FluentValidation;

namespace Florists.Application.Features.Users.Commands.CreateUser
{
  public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
  {
    private readonly IPasswordService _passwordService;
    public CreateUserCommandValidator(IPasswordService passwordService)
    {
      _passwordService = passwordService;

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
        .IsInEnum()
        .WithMessage(Messages.Users.RoleIsInvalid)
        .Must(x => IsNotAdminRole(x))
        .WithMessage(Messages.Users.UnauthorizedToModifyAdmin);

    }
    private bool IsNotAdminRole(RoleTypeOptions roleType)
    {
      return roleType != RoleTypeOptions.Admin;
    }
  }
}
