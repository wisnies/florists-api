﻿using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Auth.Commands.Logout
{
  public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
  {
    public LogoutCommandValidator()
    {
      RuleFor(x => x.Email)
        .NotEmpty()
        .WithMessage(Messages.Email.IsRequired)
        .EmailAddress()
        .WithMessage(Messages.Email.IsInvalid);
    }
  }
}
