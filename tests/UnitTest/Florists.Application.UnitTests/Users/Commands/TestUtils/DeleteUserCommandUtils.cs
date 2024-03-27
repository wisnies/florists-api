using Florists.Application.Features.Users.Commands.DeleteUser;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Users.Commands.TestUtils
{
  public static class DeleteUserCommandUtils
  {
    public static DeleteUserCommand CreateCommand()
    {
      return new DeleteUserCommand(Guid.Parse(Constants.Users.UserId));
    }

    public static FloristsUser CreateUser(bool isAdmin = false)
    {
      return new FloristsUser
      {
        UserId = Guid.Parse(Constants.Users.UserId),
        Email = Constants.Users.Email,
        FirstName = Constants.Users.FirstName,
        LastName = Constants.Users.LastName,
        CreatedAt = DateTime.Parse(Constants.Users.UtcNow),
        Role = new FloristsRole
        {
          RoleId = Guid.Parse(Constants.Users.RoleId),
          UserId = Guid.Parse(Constants.Users.UserId),
          RoleType = isAdmin ? Constants.Users.AdminRoleType : Constants.Users.RoleType,
          CreatedAt = DateTime.Parse(Constants.Users.UtcNow),
        }
      };
    }
  }
}
