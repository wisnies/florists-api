namespace Florists.Core.Common.Messages
{
  public static partial class Messages
  {
    public static class Auth
    {
      public const string InvalidCredentials = "Invalid user credentials";

      public const string JwtTokenIsRequired = "Jwt token is required";
      public const string JwtTokenIsInvalid = "Jwt token is invalid";
      public const string RefreshTokenIsRequired = "Refresh token is required";
      public const string InvalidRefreshToken = "Refresh token is invalid, User has been logged out";
      public const string RefreshTokenExpired = "Refresh token is expired, User has been logged out";

      public const string PasswordIsRequired = "Password is required";
      public const string PasswordIsInvalid = "Password is invalid";
      public const string PasswordMinLengthIs = "Password minimum length is ";
      public const string PasswordMaxLengthIs = "Password maximum length is ";
      public const string PasswordMustHaveUppercase = "Password must contain an uppercase character";
      public const string PasswordMustHaveLowercase = "Password must contain a lowercase character";
      public const string PasswordMustHaveDigit = "Password must contain a digit";
      public const string PasswordMustHaveSpecial = "Password must contain a special character";
      public const string PasswordsMustMatch = "Passwords must match";
      public const string NewPasswordIsRequired = "New password is required";
      public const string ConfirmPasswordIsRequired = "Confrim password is required";
    }
  }
}
