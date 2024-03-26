using ErrorOr;

namespace Florists.Core.Common.CustomErrors
{
  public static partial class CustomErrors
  {
    public static class Users
    {

      public static Error UnauthorizedToModifyAdmin =>
        Error.Unauthorized(
          code: "Unauthorized.RoleType",
          description: Messages.Messages.Users.UnauthorizedToModifyAdmin);


      public static Error NotFound =>
        Error.NotFound(
          code: "NotFound.User",
          description: Messages.Messages.Users.NotFound);
    }
  }
}
