using Florists.Core.Entities;

namespace Florists.Core.DTO.User
{
  public record UserResultDTO(
    string Message,
    FloristsUser User);
}
