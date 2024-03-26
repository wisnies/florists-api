namespace Florists.Core.DTO.Flowers
{
  public record RequiredInventoryDTO(
    Guid InventoryId,
    int RequiredQuantity);
}
