using Florists.Core.Enums;

namespace Florists.Core.DTO.Common
{
  public record UserDTO(
      Guid UserId,
      bool IsPasswordChanged,
      string FirstName,
      string LastName,
      string Email,
      RoleTypeOptions RoleType);
}
