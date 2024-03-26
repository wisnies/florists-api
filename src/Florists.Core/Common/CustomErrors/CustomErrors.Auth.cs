using ErrorOr;

namespace Florists.Core.Common.CustomErrors
{
  public static partial class CustomErrors
  {
    public static class Auth
    {
      public static Error InvalidCredentials =>
        Error.Validation(
          code: "Validation.Email",
          description: Messages.Messages.Auth.InvalidCredentials);

      public static Error PasswordIsInvalid =>
        Error.Validation(
          code: "Validation.Password",
          description: Messages.Messages.Auth.PasswordIsInvalid);


      public static Error EmailDuplicate =>
        Error.Validation(
          code: "Validation.Email",
          description: Messages.Messages.Email.IsDuplicate);
    }
  }
}
