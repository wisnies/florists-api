using Florists.Core.Common.Messages;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Florists.Application.Features.Auth.Commands.ChangePassword
{
  public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
  {
    public ChangePasswordCommandValidator()
    {
      RuleFor(x => x.Password)
        .NotEmpty()
        .WithMessage(Messages.Auth.PasswordIsRequired);

      RuleFor(x => x.NewPassword)
        .NotEmpty()
        .WithMessage(Messages.Auth.NewPasswordIsRequired)
        .MinimumLength(8)
        .WithMessage(Messages.Auth.PasswordMinLengthIs + 8)
        .MaximumLength(32)
        .WithMessage(Messages.Auth.PasswordMaxLengthIs + 32)
        .Must(x => IsLowercase(x))
        .WithMessage(Messages.Auth.PasswordMustHaveLowercase)
        .Must(x => IsUppercase(x))
        .WithMessage(Messages.Auth.PasswordMustHaveUppercase)
        .Must(x => IsDigit(x))
        .WithMessage(Messages.Auth.PasswordMustHaveDigit)
        .Must(x => IsSpecial(x))
        .WithMessage(Messages.Auth.PasswordMustHaveSpecial)
        .Equal(x => x.ConfirmNewPassword)
        .WithMessage(Messages.Auth.PasswordsMustMatch);

      RuleFor(x => x.ConfirmNewPassword)
        .NotEmpty()
        .WithMessage(Messages.Auth.ConfirmPasswordIsRequired);

      RuleFor(x => x.Email)
        .NotEmpty()
        .WithMessage(Messages.Email.IsRequired)
        .EmailAddress()
        .WithMessage(Messages.Email.IsInvalid);
    }

    private bool IsLowercase(string password)
    {
      var regex = new Regex("[a-z]+");
      return regex.IsMatch(password);
    }

    private bool IsUppercase(string password)
    {
      var regex = new Regex("[A-Z]+");
      return regex.IsMatch(password);
    }

    private bool IsDigit(string password)
    {
      var regex = new Regex("(\\d)+");
      return regex.IsMatch(password);
    }

    private bool IsSpecial(string password)
    {
      var regex = new Regex("(\\W)+");
      return regex.IsMatch(password);
    }
  }
}
