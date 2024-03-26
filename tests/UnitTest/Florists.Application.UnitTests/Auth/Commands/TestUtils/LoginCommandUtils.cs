using Florists.Application.Features.Auth.Commands.Login;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.DTO.Auth;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Auth.Commands.TestUtils
{
  public static class LoginCommandUtils
  {
    public static LoginCommand CreateCommand()
    {
      return new LoginCommand(
         Constants.Auth.Email,
        Constants.Auth.Password);
    }

    public static FloristsUser CreateUser()
    {
      return new FloristsUser
      {
        UserId = Guid.Parse(Constants.Auth.UserId),
        IsActive = Constants.Auth.IsActive,
        IsPasswordChanged = Constants.Auth.IsPasswordChanged,
        Email = Constants.Auth.Email,
        FirstName = Constants.Auth.FirstName,
        LastName = Constants.Auth.LastName,
        PasswordHash = Constants.Auth.PasswordHash,
        Role = new FloristsRole
        {
          RoleId = Guid.Parse(Constants.Auth.RoleId),
          IsActive = Constants.Auth.IsActive,
          RoleType = Constants.Auth.RoleType,
        }
      };
    }

    public static UserTokensDTO CreateUserTokensDTO()
    {
      return new UserTokensDTO(
        Constants.Auth.JwtToken,
        DateTime.Parse(Constants.Auth.JwtTokenExpiration),
        Constants.Auth.RefreshToken,
        DateTime.Parse(Constants.Auth.RefreshTokenExpiration));
    }
  }
}
