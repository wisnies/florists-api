using Florists.Core.Entities;

namespace Florists.Core.DTO.User
{
  public record UsersResultDTO(
    string Message,
    List<FloristsUser> Users);
}
