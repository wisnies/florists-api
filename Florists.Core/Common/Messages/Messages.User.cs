namespace Florists.Core.Common.Messages
{
  public static partial class Messages
  {
    public static class User
    {
      public const string UnableToCreateUser = "Unable to create user, try again later";
      public const string UnableToCreateUserRole = "Unable to create user role, try again later";
      public const string UnableToFetchUsers = "Unable to fetch users, try again later";
      public const string UnableToDeleteUser = "Unable to delete user, try again later";
      public const string UnableToUpdateUser = "Unable to update user, try again later";
      public const string UnableToUpdateUserRole = "Unable to update user role, try again later";
      public const string UnableToEditAdminRole = "Unable to change admin role, try again later";

      public const string InvalidUserRole = "Invalid user role";
      public const string UnableToCreateAdminRole = "Unable to create admin role";

      public const string FirstNameIsRequired = "First name is required";
      public const string FirstNameMinLengthIs = "First name minimum length is ";
      public const string FirstNameMaxLengthIs = "First name maximum length is ";

      public const string LastNameIsRequired = "Last name is required";
      public const string LastNameMinLengthIs = "Last name minimum length is ";
      public const string LastNameMaxLengthIs = "Last name maximum length is ";

      public const string RoleIsRequired = "User role is required";

      public const string UserIdIsRequired = "User id is required";

      public const string NotFound = "User not found";

      public const string CreateSuccess = "User created";
      public const string FetchSuccess = "Data fetched";
      public const string DeleteSuccess = "User Deleted";
      public const string UpdateSuccess = "User Updated";
    }
  }
}
