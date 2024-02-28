using Florists.Core.Entities;

namespace Florists.Application.Interfaces.Persistence
{
  public interface IFlowerRepository
  {
    Task<bool> CreateAsync(Flower flower);
    Task<List<Flower>?> GetManyByNameAsync(string flowerName, int offset, int perPage);
    Task<Flower?> GetOneByIdAsync(Guid flowerId);
    Task<Flower?> GetOneByNameAsync(string flowerName);
    Task<bool> UpdateAsync(Flower flower);
  }
}
