using Florists.Core.Entities;

namespace Florists.Core.Contracts.Inventories
{
  public record InventoryResponse(
    string Message,
    Inventory Inventory);
}
