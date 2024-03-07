using Florists.Core.DTO.Common;

namespace Florists.Core.Contracts.User
{
  public record UsersResponse(
    string Message,
    int Count,
    List<UserDTO> Users);
}
