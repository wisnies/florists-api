using Florists.Core.DTO.Auth;
using Florists.Core.Entities;

namespace Florists.Application.Interfaces.Persistence
{
  public interface IUserRepository
  {
    Task<FloristsUser?> GetOneByEmailAsync(string email);
    Task<FloristsUser?> GetOneByIdAsync(Guid userId);
    Task<List<FloristsUser>?> GetManyByLastNameAsync(string lastName, int offset, int perPage);
    Task<bool> AuthenticateAsync(FloristsUser user, UserTokensDTO userTokens);
    Task<FloristsUser?> GetOneByRefreshTokenAsync(string refreshToken);
    Task<bool> LogoutAsync(FloristsUser user);
    Task<bool> ChangePasswordAsync(FloristsUser user);
    Task<bool> CreateAsync(FloristsUser user);
    Task<bool> Delete(FloristsUser user);
    Task<bool> DeleteSoft(FloristsUser user);
    Task<bool> UpdateAsync(FloristsUser user);
  }
}
