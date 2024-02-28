namespace Florists.Core.Contracts.Flowers
{
  public record EditFlowerRequest(
    string FlowerName,
    int AvailableQuantity,
    double UnitPrice);
}
