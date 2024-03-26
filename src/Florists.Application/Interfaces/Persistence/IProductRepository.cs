using Florists.Core.Entities;

namespace Florists.Application.Interfaces.Persistence
{
  public interface IProductRepository
  {
    Task<int> CountByNameAsync(string? productName);
    Task<bool> CreateAsync(Product product);
    Task<List<Product>?> GetManyByNameAsync(
      int offset,
      int perPage,
      string? productName);
    Task<Product?> GetOneByIdAsync(Guid productId, bool withInventories = false);
    Task<Product?> GetOneByNameAsync(string productName, bool withInventories = false);
    Task<bool> ProduceAsync(
      ProductTransaction productTransaction,
      List<InventoryTransaction> inventoryTransactions);
    Task<bool> SoftDeleteAsync(Product productToDelete);
    Task<bool> UpdateAsync(Product productToUpdate);
  }
}
