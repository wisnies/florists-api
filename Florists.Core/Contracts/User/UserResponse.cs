using Florists.Core.DTO.Common;

namespace Florists.Core.Contracts.User
{
  public record UserResponse(
    string Message,
    UserDTO User);
}
