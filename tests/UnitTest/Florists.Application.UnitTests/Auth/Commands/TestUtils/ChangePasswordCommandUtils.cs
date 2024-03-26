using Florists.Application.Features.Auth.Commands.ChangePassword;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Auth.Commands.TestUtils
{
  public static class ChangePasswordCommandUtils
  {
    public static ChangePasswordCommand CreateCommand()
    {
      return new ChangePasswordCommand(
        Constants.Auth.Password,
        Constants.Auth.NewPassword,
        Constants.Auth.NewPassword,
        Constants.Auth.Email);
    }

    public static FloristsUser CreateUser()
    {
      return new FloristsUser
      {
        UserId = Guid.Parse(Constants.Auth.UserId),
        IsActive = Constants.Auth.IsActive,
        IsPasswordChanged = false,
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
