using Florists.Core.Enums;

namespace Florists.Core.Contracts.User
{
  public record EditUserRequest(
    string Email,
    string FirstName,
    string LastName,
    RoleTypeOptions RoleType);
}
