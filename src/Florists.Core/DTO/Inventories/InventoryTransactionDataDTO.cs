using Florists.Core.Enums;

namespace Florists.Core.DTO.Inventories
{
  public record InventoryTransactionDataDTO(
    Guid InventoryId,
    string InventoryName,
    double UnitPrice,
    InventoryCategoryOptions Category);
}
