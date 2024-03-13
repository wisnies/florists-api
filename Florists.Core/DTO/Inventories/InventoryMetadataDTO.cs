using Florists.Core.Enums;

namespace Florists.Core.DTO.Inventories
{
  public record InventoryMetadataDTO(
    Guid InventoryId,
    string InventoryName,
    double UnitPrice,
    InventoryCategoryOptions Category);
}
