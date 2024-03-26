using Florists.Core.Entities;

namespace Florists.Core.DTO.Products
{
  public record ProductsResultDTO(
    string Message,
    int Count,
    List<Product> Products);
}
