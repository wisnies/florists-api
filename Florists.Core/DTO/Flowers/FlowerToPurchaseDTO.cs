namespace Florists.Core.DTO.Flowers
{
  public record FlowerToPurchaseDTO(
    Guid FlowerId,
    int QuantityToPurchase);
}
