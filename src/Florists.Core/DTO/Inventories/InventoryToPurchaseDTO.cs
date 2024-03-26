namespace Florists.Core.DTO.Flowers
{
  public record InventoryToPurchaseDTO(
    Guid InventoryId,
    int QuantityToPurchase);
}
