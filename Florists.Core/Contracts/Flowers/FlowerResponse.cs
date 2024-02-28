using Florists.Core.Entities;

namespace Florists.Core.Contracts.Flowers
{
  public record FlowerResponse(
    string Message,
    Flower Flower);
}
