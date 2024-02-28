namespace Florists.Core.Contracts.Flowers
{
  public record GetFlowersByNameRequest(
    string? FlowerName,
    int Page = 1,
    int PerPage = 10);
}
