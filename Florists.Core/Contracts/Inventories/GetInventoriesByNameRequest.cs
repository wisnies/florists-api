namespace Florists.Core.Contracts.Inventories
{
  public record GetInventoriesByNameRequest(
    string? InventoryName,
    int Page = 1,
    int PerPage = 10);
}
