using Florists.Core.Entities;

namespace Florists.Core.DTO.Auth
{
  public record AuthResultDTO(
    FloristsUser User,
    UserTokensDTO Tokens);
}
