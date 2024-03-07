using Florists.Core.Entities;

namespace Florists.Application.Interfaces.Persistence
{
  public interface IProductRepository
  {
    Task<bool> CreateAsync(Product product);
    Task<Product?> GetOneByNameAsync(string productName, bool withInventories = false);
  }
}
