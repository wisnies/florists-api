using Florists.Application.Features.Auth.Commands.Logout;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Auth.Commands.TestUtils
{
  public static class LogoutCommandUtils
  {
    public static LogoutCommand CreateCommand()
    {
      return new LogoutCommand(Constants.Auth.Email);
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
  }
}
