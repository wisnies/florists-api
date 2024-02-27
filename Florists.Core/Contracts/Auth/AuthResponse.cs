using Florists.Core.DTO.Auth;
using Florists.Core.Entities;

namespace Florists.Core.Contracts.Auth
{
  public record AuthResponse(
  FloristsUser User,
  UserTokensDTO? Tokens);
}
