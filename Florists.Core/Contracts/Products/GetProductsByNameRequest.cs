namespace Florists.Core.Contracts.Products
{
  public record GetProductsByNameRequest(
    string? ProductName,
    int Page = 1,
    int PerPage = 10);
}
