using Florists.Core.Entities;

namespace Florists.Core.Contracts.User
{
  public record UserResponse(
    string Message,
    FloristsUser User);
}
