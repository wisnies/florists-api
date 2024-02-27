using ErrorOr;

namespace Florists.Core.Common.CustomErrors
{
  public static partial class CustomErrors
  {
    public static class User
    {
      public static Error UnableToCreateUser =>
        Error.Failure(
          code: "User.UnableToCreateUser",
          description: Messages.Messages.User.UnableToCreateUser);

      public static Error UnableToCreateUserRole =>
        Error.Failure(
          code: "User.UnableToCreateUserRole",
          description: Messages.Messages.User.UnableToCreateUserRole);

      public static Error UnableToFetchUsers =>
        Error.Failure(
          code: "User.UnableToFetchUsers",
          description: Messages.Messages.User.UnableToFetchUsers);

      public static Error UnableToDeleteUser =>
        Error.Failure(
          code: "User.UnableToDeleteUser",
          description: Messages.Messages.User.UnableToDeleteUser);

      public static Error UnableToUpdateUserRole =>
        Error.Failure(
          code: "User.UnableToUpdateRole",
          description: Messages.Messages.User.UnableToUpdateUserRole);

      public static Error UnableToUpdateUser =>
        Error.Failure(
          code: "User.UnableToUpdateUser",
          description: Messages.Messages.User.UnableToUpdateUser);

      public static Error UnableToEditAdminRole =>
        Error.Failure(
          code: "User.UnableToEditAdminRole",
          description: Messages.Messages.User.UnableToEditAdminRole);


      public static Error NotFound =>
        Error.NotFound(
          code: "User.NotFound",
          description: Messages.Messages.User.NotFound);
    }
  }
}
