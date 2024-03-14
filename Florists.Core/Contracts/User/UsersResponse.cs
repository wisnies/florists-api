using Florists.Core.DTO.User;

namespace Florists.Core.Contracts.User
{
    public record UsersResponse(
    string Message,
    int Count,
    List<UserDTO> Users);
}
