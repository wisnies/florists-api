namespace Florists.Core.Contracts.Products
{
  public record ProduceProductRequest(
    Guid ProductId,
    int QuantityToProduce,
    string ProductionOrderNumber);
}
