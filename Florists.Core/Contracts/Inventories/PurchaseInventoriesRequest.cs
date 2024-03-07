namespace Florists.Core.Contracts.Inventories
{
  public record PurchaseInventoriesRequest(
    string PurchaseOrderNumber,
    List<InventoryToPurchase> InventoriesToPurchase);

  public record InventoryToPurchase(
    Guid InventoryId,
    int QuantityToPurchase);
}
