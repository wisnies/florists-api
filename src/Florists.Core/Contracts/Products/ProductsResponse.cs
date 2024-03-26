using Florists.Core.Entities;

namespace Florists.Core.Contracts.Products
{
  public record ProductsResponse(
    string Message,
    int Count,
    List<Product> Products);
}
