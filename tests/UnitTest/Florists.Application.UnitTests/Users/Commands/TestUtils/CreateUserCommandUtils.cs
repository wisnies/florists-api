using Florists.Application.Features.Users.Commands.CreateUser;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Users.Commands.TestUtils
{
  public static class CreateUserCommandUtils
  {
    public static CreateUserCommand CreateCommand()
    {
      return new CreateUserCommand(
        Constants.Users.Email,
        Constants.Users.FirstName,
        Constants.Users.LastName,
        Constants.Users.Password,
        Constants.Users.Password,
        Constants.Users.RoleType
        );
    }

    public static FloristsUser CreateUser()
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
          RoleType = Constants.Users.RoleType,
          CreatedAt = DateTime.Parse(Constants.Users.UtcNow),
        }
      };
    }
  }
}
