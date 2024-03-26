using Florists.Core.Entities;

namespace Florists.Core.Contracts.Products
{
  public record ProductResponse(
    string Message,
    Product Product);
}
