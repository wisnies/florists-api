using Florists.Core.Entities;

namespace Florists.Application.Interfaces.Persistence
{
  public interface IProductRepository
  {
    Task<int> CountByNameAsync(string productName);
    Task<bool> CreateAsync(Product product);
    Task<List<Product>?> GetManyByNameAsync(string productName, int offset, int perPage);
    Task<Product?> GetOneByIdAsync(Guid productId, bool withInventories = false);
    Task<Product?> GetOneByNameAsync(string productName, bool withInventories = false);
    Task<bool> SoftDeleteAsync(Product productToDelete);
  }
}
