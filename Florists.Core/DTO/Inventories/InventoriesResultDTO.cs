using Florists.Core.Entities;

namespace Florists.Core.DTO.Inventories
{
  public record InventoriesResultDTO(
    string Message,
    int Count,
    List<Inventory> Inventories);
}
