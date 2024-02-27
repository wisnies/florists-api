using Florists.Core.Entities;

namespace Florists.Core.Contracts.User
{
  public record UsersResponse(
    string Message,
    List<FloristsUser> Users);
}
