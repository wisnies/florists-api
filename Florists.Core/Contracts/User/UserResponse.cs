using Florists.Core.DTO.User;

namespace Florists.Core.Contracts.User
{
    public record UserResponse(
    string Message,
    UserDTO User);
}
