namespace Florists.Core.Contracts.Products
{
  public record GetProductsByNameRequest(
    string? ProductName,
    int Page,
    int PerPage);
}
