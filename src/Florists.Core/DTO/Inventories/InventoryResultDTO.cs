using Florists.Core.Entities;

namespace Florists.Core.DTO.Inventories
{
  public record InventoryResultDTO(
    string Message,
    Inventory Inventory);
}
