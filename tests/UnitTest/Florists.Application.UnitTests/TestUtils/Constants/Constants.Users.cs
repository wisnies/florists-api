using Florists.Core.Enums;

namespace Florists.Application.UnitTests.TestUtils.Constants
{
  public static partial class Constants
  {
    public static class Users
    {
      public const string UserId = "E4C92608-6377-40F0-AE37-55D20525CCD9";
      public const string Email = "test@email.com";
      public const string EditedEmail = "test@email.com";
      public const string FirstName = "first";
      public const string EditedFirstName = "first";
      public const string LastName = "last";
      public const string EditedLastName = "last";
      public const string Password = "P@s5w0rD";
      public const string PasswordHash = "passwordHash";
      public const RoleTypeOptions RoleType = RoleTypeOptions.Demo;
      public const RoleTypeOptions EditedRoleType = RoleTypeOptions.Sales;
      public const RoleTypeOptions AdminRoleType = RoleTypeOptions.Admin;

      public const string UtcNow = "25/3/2024 09:00:00 AM";
      public const string EditedUtcNow = "26/3/2024 09:00:00 AM";

      public const string RoleId = "3E971209-5B1C-4267-8029-49FA741F747E";

      public const int UsersCount = 12;
      public static string EmailFromIndex(int index) => $"{Email} {index}";
    }
  }
}
