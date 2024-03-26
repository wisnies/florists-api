using Florists.Core.Entities;

namespace Florists.Application.Interfaces.Persistence
{
  public interface IInventoryRepository
  {
    Task<bool> CreateAsync(Inventory inventory);
    Task<List<Inventory>?> GetManyByNameAsync(
      int offset,
      int perPage,
      string? inventoryName);
    Task<Inventory?> GetOneByIdAsync(Guid inventoryId);
    Task<Inventory?> GetOneByNameAsync(string inventoryName);
    Task<bool> UpdateAsync(Inventory inventory);
    Task<int> CountByNameAsync(string? inventoryName);
  }
}
