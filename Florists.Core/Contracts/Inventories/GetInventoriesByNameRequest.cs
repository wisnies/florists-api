namespace Florists.Core.Contracts.Inventories
{
  public record GetInventoriesByNameRequest(
    string? InventoryName,
    int Page,
    int PerPage);
}
