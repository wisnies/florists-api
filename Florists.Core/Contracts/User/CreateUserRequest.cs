using Florists.Core.Enums;

namespace Florists.Core.Contracts.User
{
  public record CreateUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string ConfirmPassword,
    RoleTypeOptions RoleType);
}
