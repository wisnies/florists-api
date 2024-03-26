using Florists.Core.Entities;

namespace Florists.Core.Contracts.Inventories
{
  public record InventoriesResponse(
    string Message,
    int Count,
    List<Inventory> Inventories);
}
