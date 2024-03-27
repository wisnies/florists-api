using Florists.Application.Features.Auth.Commands.ChangePassword;
using Florists.Application.Features.Auth.Commands.Login;
using Florists.Application.Features.Auth.Commands.Logout;
using Florists.Application.Features.Auth.Commands.RefreshToken;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Auth;
using Florists.Core.DTO.Common;

namespace Florists.Application.UnitTests.TestUtils.Auth.Extenstions
{
  public static partial class AuthExtensions
  {
    public static void ValidateCreatedFrom(this AuthResultDTO result, LoginCommand command)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.User.UserId, Is.EqualTo(Guid.Parse(Constants.Constants.Auth.UserId)));
        Assert.That(result.User.Email, Is.EqualTo(Constants.Constants.Auth.Email));
        Assert.That(result.User.FirstName, Is.EqualTo(Constants.Constants.Auth.FirstName));
        Assert.That(result.User.LastName, Is.EqualTo(Constants.Constants.Auth.LastName));

        Assert.That(result.User.Role, Is.Not.Null);
        Assert.That(result.User.Role!.RoleId, Is.EqualTo(Guid.Parse(Constants.Constants.Auth.RoleId)));
        Assert.That(result.User.Role!.RoleType, Is.EqualTo(Constants.Constants.Auth.RoleType));

        Assert.That(result.Tokens.JwtToken, Is.EqualTo(Constants.Constants.Auth.JwtToken));
        Assert.That(result.Tokens.RefreshToken, Is.EqualTo(Constants.Constants.Auth.RefreshToken));
        Assert.That(result.Tokens.JwtTokenExpiration, Is.EqualTo(DateTime.Parse(Constants.Constants.Auth.JwtTokenExpiration)));
        Assert.That(result.Tokens.RefreshTokenExpiration, Is.EqualTo(DateTime.Parse(Constants.Constants.Auth.RefreshTokenExpiration)));
      });
    }

    public static void ValidateCreatedFrom(this MessageResultDTO result, LogoutCommand command)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Message, Is.EqualTo(Messages.Database.SaveSuccess));
      });
    }

    public static void ValidateCreatedFrom(this MessageResultDTO result, ChangePasswordCommand command)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Message, Is.EqualTo(Messages.Database.SaveSuccess));
      });
    }

    public static void ValidateCreatedFrom(this AuthResultDTO result, RefreshTokenCommand command)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.User.UserId, Is.EqualTo(Guid.Parse(Constants.Constants.Auth.UserId)));
        Assert.That(result.User.Email, Is.EqualTo(Constants.Constants.Auth.Email));
        Assert.That(result.User.FirstName, Is.EqualTo(Constants.Constants.Auth.FirstName));
        Assert.That(result.User.LastName, Is.EqualTo(Constants.Constants.Auth.LastName));

        Assert.That(result.User.Role, Is.Not.Null);
        Assert.That(result.User.Role!.RoleId, Is.EqualTo(Guid.Parse(Constants.Constants.Auth.RoleId)));
        Assert.That(result.User.Role!.RoleType, Is.EqualTo(Constants.Constants.Auth.RoleType));

        Assert.That(result.Tokens.JwtToken, Is.EqualTo(Constants.Constants.Auth.JwtToken));
        Assert.That(result.Tokens.RefreshToken, Is.EqualTo(Constants.Constants.Auth.RefreshToken));
        Assert.That(result.Tokens.JwtTokenExpiration, Is.EqualTo(DateTime.Parse(Constants.Constants.Auth.JwtTokenExpiration)));
        Assert.That(result.Tokens.RefreshTokenExpiration, Is.EqualTo(DateTime.Parse(Constants.Constants.Auth.RefreshTokenExpiration)));
      });
    }
  }
}
