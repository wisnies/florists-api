using Florists.Core.Entities;

namespace Florists.Application.Interfaces.Persistence
{
  public interface IRoleRepository
  {
    Task<bool> CreateAsync(FloristsRole role);
    Task<bool> UpdateAsync(FloristsRole role);
  }
}
