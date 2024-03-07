using Florists.Core.Entities;

namespace Florists.Application.Interfaces.Persistence
{
  public interface IInventoryRepository
  {
    Task<bool> CreateAsync(Inventory inventory);
    Task<List<Inventory>?> GetManyByNameAsync(string inventoryName, int offset, int perPage);
    Task<Inventory?> GetOneByIdAsync(Guid inventoryId);
    Task<Inventory?> GetOneByNameAsync(string inventoryName);
    Task<bool> UpdateAsync(Inventory inventory);
    Task<int> CountByNameAsync(string inventoryName);
  }
}
