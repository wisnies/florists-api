using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Auth.Commands.Login
{
  public class LoginCommandValidator : AbstractValidator<LoginCommand>
  {
    public LoginCommandValidator()
    {
      RuleFor(x => x.Email)
        .NotEmpty()
        .WithMessage(Messages.Auth.EmailIsRequired)
        .EmailAddress()
        .WithMessage(Messages.Auth.EmailMustBeValid);
      RuleFor(x => x.Password)
        .NotEmpty()
        .WithMessage(Messages.Auth.PasswordIsRequired);
    }
  }
}
