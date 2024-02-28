using Florists.Core.Entities;

namespace Florists.Core.Contracts.Flowers
{
  public record FlowersResponse(
    string Message,
    List<Flower> Flowers);
}
