using Florists.Application.Features.Users.Queries.GetUsersByLastName;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Users.Queries.TestUtils
{
  public static class GetUsersByLastNameQueryUtils
  {
    public static GetUsersByLastNameQuery CreateQuery(int perPage = 10, int page = 1)
    {
      return new GetUsersByLastNameQuery(
        Constants.Users.LastName,
        page,
        perPage);
    }

    public static List<FloristsUser> CreateUsers(int offset, int perPage)
    {
      var users = new List<FloristsUser>();

      var total = perPage >= Constants.Users.UsersCount ? Constants.Users.UsersCount : perPage;
      total += offset;

      for (int i = offset; i < total; i++)
      {
        var user = new FloristsUser
        {
          UserId = Guid.Parse(Constants.Users.UserId),
          Email = Constants.Users.EmailFromIndex(i),
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
        users.Add(user);
      }

      return users;
    }
  }
}
