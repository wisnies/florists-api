using Florists.Application.Features.Users.Queries.GetUserById;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Users.Queries.TestUtils
{
  public static class GetUserByIdQueryUtils
  {
    public static GetUserByIdQuery CreateQuery()
    {
      return new GetUserByIdQuery(Guid.Parse(Constants.Users.UserId));
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
