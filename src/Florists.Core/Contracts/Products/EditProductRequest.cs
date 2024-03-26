using Florists.Core.Enums;

namespace Florists.Core.Contracts.Products
{
  public record EditProductRequest(
    string ProductName,
    double UnitPrice,
    string Sku,
    ProductCategoryOptions Category,
    List<RequiredInventory> RequiredInventories);
}
