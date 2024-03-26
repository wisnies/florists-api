using Florists.Application.Features.Auth.Commands.RefreshToken;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.DTO.Auth;
using Florists.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Florists.Application.UnitTests.Auth.Commands.TestUtils
{
  public static class RefreshTokenCommandUtils
  {
    public static RefreshTokenCommand CreateCommand(bool validRefreshToken = true)
    {
      return new RefreshTokenCommand(
         Constants.Auth.JwtToken,
         validRefreshToken ? Constants.Auth.RefreshToken : "");
    }

    public static FloristsUser CreateUser(bool expiredToken = false)
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
        RefreshToken = Constants.Auth.RefreshToken,
        RefreshTokenExpiration = expiredToken ?
          DateTime.Parse(Constants.Auth.ExpiredRefreshTokenExpiration) :
          DateTime.Parse(Constants.Auth.ValidRefreshTokenExpiration),
        Role = new FloristsRole
        {
          RoleId = Guid.Parse(Constants.Auth.RoleId),
          IsActive = Constants.Auth.IsActive,
          RoleType = Constants.Auth.RoleType,
        }
      };
    }

    public static ClaimsPrincipal CreateClaimsPrincipal(FloristsUser user, bool withEmail = true)
    {
      var claims = new Claim[] {
        new Claim(JwtRegisteredClaimNames.Sub, Constants.Auth.UserId),
        new Claim(JwtRegisteredClaimNames.Jti, Constants.Auth.TokenJti),
        new Claim(ClaimTypes.Name, user.FirstName + user.LastName),
        new Claim(withEmail ? ClaimTypes.Email : ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, user.Role!.RoleType.ToString())};



      var claimsIdentity = new ClaimsIdentity();

      foreach (var claim in claims)
      {
        claimsIdentity.AddClaim(claim);
      }

      var principal = new ClaimsPrincipal();
      principal.AddIdentity(claimsIdentity);
      return principal;
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
