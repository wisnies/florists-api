using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.DTO.Auth;
using MediatR;

namespace Florists.Application.Features.Auth.Commands.Login
{
  public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<AuthResultDTO>>
  {
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordService _passwordService;

    public LoginCommandHandler(
      IUserRepository userRepository,
      ITokenService tokenService,
      IPasswordService passwordService)
    {
      _userRepository = userRepository;
      _tokenService = tokenService;
      _passwordService = passwordService;
    }

    public async Task<ErrorOr<AuthResultDTO>> Handle(
      LoginCommand command,
      CancellationToken cancellationToken)
    {


      var dbUser = await _userRepository.GetOneByEmailAsync(command.Email);

      if (dbUser is null)
      {
        return CustomErrors.Auth.InvalidCredentials;
      }

      var isPasswordValid = await _passwordService.IsValidAsync(command.Password, dbUser.PasswordHash);

      if (!isPasswordValid)
      {
        return CustomErrors.Auth.InvalidCredentials;
      }

      var userTokens = _tokenService.GenerateToken(dbUser);

      var success = await _userRepository.AuthenticateAsync(
        dbUser,
        userTokens);

      if (!success)
      {
        return CustomErrors.Auth.UnableToLogin;
      }

      return new AuthResultDTO(dbUser, userTokens);
    }
  }
}
