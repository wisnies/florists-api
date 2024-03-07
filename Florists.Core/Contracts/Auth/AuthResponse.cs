using Florists.Core.DTO.Auth;
using Florists.Core.DTO.Common;

namespace Florists.Core.Contracts.Auth
{
  public record AuthResponse(
  UserDTO User,
  AuthTokenDTO Tokens);
}
