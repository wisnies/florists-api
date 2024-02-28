namespace Florists.Core.Contracts.Flowers
{
  public record CreateFlowerRequest(
    string FlowerName,
    int AvailableQuantity,
    double UnitPrice);
}
