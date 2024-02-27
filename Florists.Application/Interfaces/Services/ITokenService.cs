using Florists.Core.DTO.Auth;
using Florists.Core.Entities;
using System.Security.Claims;

namespace Florists.Application.Interfaces.Services
{
  public interface ITokenService
  {
    UserTokensDTO GenerateToken(FloristsUser user);
    ClaimsPrincipal? GetClaimsPrincipal(string token);
    bool IsValid(string token);
  }
}
