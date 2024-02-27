namespace Florists.Core.Common.Messages
{
  public static partial class Messages
  {
    public static class Auth
    {
      public const string InvalidCredentials = "Invalid user credentials";
      public const string UnableToLogin = "Unable to login, try again later";
      public const string UnableToAuthenticate = "Unable to authenticate, try again later";
      public const string UnableToLogout = "Unable to logout, try again later";
      public const string UnableToChangePassword = "Unable to change password, try again later";

      public const string EmailIsRequired = "Email address is required";
      public const string EmailMustBeValid = "This must be a valid email address";

      public const string JwtTokenIsRequired = "Jwt token is required";
      public const string JwtTokenIsInvalid = "Jwt token is invalid";
      public const string RefreshTokenIsRequired = "Refresh token is required";
      public const string InvalidRefreshToken = "Refresh token is invalid, User has been logged out";
      public const string RefreshTokenExpired = "Refresh token is expired, User has been logged out";

      public const string PasswordIsRequired = "Password is required";
      public const string PasswordMinLengthIs = "Password minimum length is ";
      public const string PasswordMaxLengthIs = "Password maximum length is ";
      public const string PasswordMustHaveUppercase = "Password must contain an uppercase character";
      public const string PasswordMustHaveLowercase = "Password must contain a lowercase character";
      public const string PasswordMustHaveDigit = "Password must contain a digit";
      public const string PasswordMustHaveSpecial = "Password must contain a special character";
      public const string PasswordsMustMatch = "Passwords must match";
      public const string NewPasswordIsRequired = "New password is required";
      public const string ConfirmPasswordIsRequired = "Confrim password is required";

      public const string LogoutSuccess = "User logged out";
      public const string PasswordChangeSuccess = "Password changed successfully, User has been logged out";
    }
  }
}
