using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.DTO.Auth;
using MediatR;
using System.Security.Claims;

namespace Florists.Application.Features.Auth.Commands.RefreshToken
{
  public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<AuthResultDTO>>
  {
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IDateTimeService _dateTimeService;

    public RefreshTokenCommandHandler(
      IUserRepository userRepository,
      ITokenService tokenService,
      IDateTimeService dateTimeService)
    {
      _userRepository = userRepository;
      _tokenService = tokenService;
      _dateTimeService = dateTimeService;
    }

    public async Task<ErrorOr<AuthResultDTO>> Handle(
      RefreshTokenCommand command,
      CancellationToken cancellationToken)
    {
      var claimsPrincipal = _tokenService.GetClaimsPrincipal(command.JwtToken);

      if (claimsPrincipal is null)
      {
        return CustomErrors.Auth.InvalidCredentials;
      }

      var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

      if (email is null)
      {
        return CustomErrors.Auth.InvalidCredentials;
      }

      var user = await _userRepository.GetOneByEmailAsync(email);

      if (user is null)
      {
        return CustomErrors.Auth.InvalidCredentials;
      }

      if (!user.RefreshToken.Equals(command.RefreshToken))
      {
        return CustomErrors.Auth.InvalidRefreshToken;
      }

      if (user.RefreshTokenExpiration < _dateTimeService.UtcNow)
      {
        return CustomErrors.Auth.RefreshTokenExpired;
      }

      var userTokens = _tokenService.GenerateToken(user);

      var success = await _userRepository.AuthenticateAsync(user, userTokens);

      if (!success)
      {
        return CustomErrors.Auth.UnableToAuthenticate;
      }

      return new AuthResultDTO(
      user,
      userTokens);
    }
  }
}
