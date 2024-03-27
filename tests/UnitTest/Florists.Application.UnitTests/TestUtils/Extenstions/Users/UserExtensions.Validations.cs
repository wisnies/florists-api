using Florists.Application.Features.Users.Commands.CreateUser;
using Florists.Application.Features.Users.Commands.DeleteUser;
using Florists.Application.Features.Users.Commands.EditUser;
using Florists.Application.Features.Users.Queries.GetUserById;
using Florists.Application.Features.Users.Queries.GetUsersByLastName;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.User;

namespace Florists.Application.UnitTests.TestUtils.Extenstions.Users
{
  public static partial class UserExtensions
  {
    public static void ValidateCreatedFrom(this UserResultDTO result, CreateUserCommand command)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.SaveSuccess));

        Assert.That(result.User.UserId, Is.TypeOf(typeof(Guid)));
        Assert.That(result.User.Email, Is.EqualTo(Constants.Constants.Users.Email));
        Assert.That(result.User.FirstName, Is.EqualTo(Constants.Constants.Users.FirstName));
        Assert.That(result.User.LastName, Is.EqualTo(Constants.Constants.Users.LastName));
        Assert.That(result.User.PasswordHash, Is.EqualTo(Constants.Constants.Users.PasswordHash));
        Assert.That(result.User.IsActive, Is.True);
        Assert.That(result.User.IsPasswordChanged, Is.False);
        Assert.That(result.User.CreatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Users.UtcNow)));

        Assert.That(result.User.Role, Is.Not.Null);
        Assert.That(result.User.Role!.RoleId, Is.TypeOf(typeof(Guid)));
        Assert.That(result.User.Role!.UserId, Is.EqualTo(result.User.UserId));
        Assert.That(result.User.Role!.RoleType, Is.EqualTo(Constants.Constants.Users.RoleType));
        Assert.That(result.User.Role!.CreatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Users.UtcNow)));
      });
    }

    public static void ValidateCreatedFrom(this UserResultDTO result, DeleteUserCommand command)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.DeleteSuccess));

        Assert.That(result.User.UserId, Is.TypeOf(typeof(Guid)));
        Assert.That(result.User.Email, Is.EqualTo(Constants.Constants.Users.Email));
        Assert.That(result.User.FirstName, Is.EqualTo(Constants.Constants.Users.FirstName));
        Assert.That(result.User.LastName, Is.EqualTo(Constants.Constants.Users.LastName));
        Assert.That(result.User.IsActive, Is.False);
        Assert.That(result.User.CreatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Users.UtcNow)));

        Assert.That(result.User.Role, Is.Not.Null);
        Assert.That(result.User.Role!.RoleId, Is.TypeOf(typeof(Guid)));
        Assert.That(result.User.Role!.UserId, Is.EqualTo(result.User.UserId));
        Assert.That(result.User.Role!.RoleType, Is.EqualTo(Constants.Constants.Users.RoleType));
        Assert.That(result.User.Role!.CreatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Users.UtcNow)));
      });
    }

    public static void ValidateCreatedFrom(this UserResultDTO result, EditUserCommand command)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.UpdateSuccess));

        Assert.That(result.User.UserId, Is.EqualTo(Guid.Parse(Constants.Constants.Users.UserId)));
        Assert.That(result.User.Email, Is.EqualTo(Constants.Constants.Users.EditedEmail));
        Assert.That(result.User.FirstName, Is.EqualTo(Constants.Constants.Users.EditedFirstName));
        Assert.That(result.User.LastName, Is.EqualTo(Constants.Constants.Users.EditedLastName));
        Assert.That(result.User.CreatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Users.UtcNow)));
        Assert.That(result.User.UpdatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Users.EditedUtcNow)));

        Assert.That(result.User.Role, Is.Not.Null);
        Assert.That(result.User.Role!.RoleId, Is.EqualTo(Guid.Parse(Constants.Constants.Users.RoleId)));
        Assert.That(result.User.Role!.UserId, Is.EqualTo(result.User.UserId));
        Assert.That(result.User.Role!.RoleType, Is.EqualTo(Constants.Constants.Users.EditedRoleType));
        Assert.That(result.User.Role!.CreatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Users.UtcNow)));
        Assert.That(result.User.Role!.UpdatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Users.EditedUtcNow)));
      });
    }

    public static void ValidateCreatedFrom(this UserResultDTO result, GetUserByIdQuery query)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.FetchSuccess));

        Assert.That(result.User.UserId, Is.EqualTo(Guid.Parse(Constants.Constants.Users.UserId)));
        Assert.That(result.User.Email, Is.EqualTo(Constants.Constants.Users.Email));
        Assert.That(result.User.FirstName, Is.EqualTo(Constants.Constants.Users.FirstName));
        Assert.That(result.User.LastName, Is.EqualTo(Constants.Constants.Users.LastName));
        Assert.That(result.User.CreatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Users.UtcNow)));


        Assert.That(result.User.Role, Is.Not.Null);
        Assert.That(result.User.Role!.RoleId, Is.EqualTo(Guid.Parse(Constants.Constants.Users.RoleId)));
        Assert.That(result.User.Role!.UserId, Is.EqualTo(result.User.UserId));
        Assert.That(result.User.Role!.RoleType, Is.EqualTo(Constants.Constants.Users.RoleType));
        Assert.That(result.User.Role!.CreatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Users.UtcNow)));
      });
    }

    public static void ValidateCreatedFrom(this UsersResultDTO result, GetUsersByLastNameQuery query, int offset)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.FetchSuccess));
        Assert.That(result.Count, Is.EqualTo(Constants.Constants.Users.UsersCount));
        Assert.That(result.Users, Has.Count.LessThanOrEqualTo(query.PerPage));

        if (query.PerPage >= Constants.Constants.Users.UsersCount)
        {
          Assert.That(result.Users, Has.Count.AtLeast(Constants.Constants.Users.UsersCount));
        }

        for (int i = 0; i < result.Users.Count; i++)
        {
          var n = offset + i;
          Assert.That(
            result.Users[i].Email,
            Is.EqualTo(Constants.Constants.Users.EmailFromIndex(n)));
        }
      });
    }
  }
}
