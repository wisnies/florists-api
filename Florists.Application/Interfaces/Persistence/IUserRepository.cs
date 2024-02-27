using Florists.Core.DTO.Auth;
using Florists.Core.Entities;

namespace Florists.Application.Interfaces.Persistence
{
  public interface IUserRepository
  {
    Task<FloristsUser?> GetOneByEmailAsync(string email);
    Task<bool> AuthenticateAsync(FloristsUser user, UserTokensDTO userTokens);
    Task<FloristsUser?> GetOneByRefreshTokenAsync(string refreshToken);
    Task<bool> LogoutAsync(FloristsUser user);
    Task<bool> ChangePasswordAsync(FloristsUser user);
  }
}
