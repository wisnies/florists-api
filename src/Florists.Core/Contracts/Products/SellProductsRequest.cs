namespace Florists.Core.Contracts.Products
{
  public record SellProductsRequest(
    string SaleOrderNumber,
    List<ProductToSell> ProductsToSell);

  public record ProductToSell(
    Guid ProductId,
    int QuantityToSell);
}
