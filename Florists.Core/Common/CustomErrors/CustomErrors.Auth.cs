using ErrorOr;

namespace Florists.Core.Common.CustomErrors
{
  public static partial class CustomErrors
  {
    public static class Auth
    {
      public static Error InvalidCredentials =>
        Error.Validation(
          code: "Auth.InvalidCredentials",
          description: Messages.Messages.Auth.InvalidCredentials);

      public static Error InvalidRefreshToken =>
        Error.Validation(
          code: "Auth.InvalidCredentials",
          description: Messages.Messages.Auth.InvalidRefreshToken);

      public static Error RefreshTokenExpired =>
        Error.Validation(
          code: "Auth.InvalidCredentials",
          description: Messages.Messages.Auth.RefreshTokenExpired);

      public static Error UnableToLogin =>
        Error.Failure(
          code: "Auth.UnableToLogin",
          description: Messages.Messages.Auth.UnableToLogin);

      public static Error UnableToAuthenticate =>
        Error.Failure(
          code: "Auth.UnableToLogin",
          description: Messages.Messages.Auth.UnableToAuthenticate);

      public static Error UnableToLogout =>
        Error.Failure(
          code: "Auth.UnableToLogout",
          description: Messages.Messages.Auth.UnableToLogout);

      public static Error UnableToChangePassword =>
        Error.Failure(
          code: "Auth.UnableToLogout",
          description: Messages.Messages.Auth.UnableToChangePassword);
    }
  }
}
