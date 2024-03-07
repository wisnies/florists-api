using Florists.Core.Enums;

namespace Florists.Core.Contracts.Products
{
  public record CreateProductRequest(
    string ProductName,
    double UnitPrice,
    string Sku,
    ProductCategoryOptions Category,
    List<RequiredInventory> RequiredInventories);

  public record RequiredInventory(
    Guid InventoryId,
    int RequiredQuantity);
}
