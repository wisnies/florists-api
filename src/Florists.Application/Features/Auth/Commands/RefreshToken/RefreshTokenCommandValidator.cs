using Florists.Application.Interfaces.Services;
using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.Auth.Commands.RefreshToken
{
  public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
  {
    private readonly ITokenService _tokenService;
    public RefreshTokenCommandValidator(ITokenService tokenService)
    {
      _tokenService = tokenService;

      RuleFor(x => x.JwtToken)
        .NotEmpty()
        .WithMessage(Messages.Auth.JwtTokenIsRequired)
        .Must(x => IsValidJwt(x))
        .WithMessage(Messages.Auth.JwtTokenIsInvalid);

      RuleFor(x => x.RefreshToken)
        .NotEmpty()
        .WithMessage(Messages.Auth.RefreshTokenIsRequired);
    }

    private bool IsValidJwt(string token)
    {
      return _tokenService.IsValid(token);
    }
  }
}
