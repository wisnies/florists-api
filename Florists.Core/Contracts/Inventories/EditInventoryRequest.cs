using Florists.Core.Enums;

namespace Florists.Core.Contracts.Inventories
{
  public record EditInventoryRequest(
    string InventoryName,
    int AvailableQuantity,
    double UnitPrice,
    InventoryCategoryOptions Category);
}
