namespace Florists.Core.Contracts.Flowers
{
  public record PurchaseFlowersRequest(
    string PurchaseOrderNumber,
    List<FlowerToPurchase> FlowersToPurchase);

  public record FlowerToPurchase(
    Guid FlowerId,
    int QuantityToPurchase);
}
