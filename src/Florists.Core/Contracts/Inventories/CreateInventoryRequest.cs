using Florists.Core.Enums;

namespace Florists.Core.Contracts.Inventories
{
  public record CreateInventoryRequest(
    string InventoryName,
    int AvailableQuantity,
    double UnitPrice,
    InventoryCategoryOptions Category);
}
