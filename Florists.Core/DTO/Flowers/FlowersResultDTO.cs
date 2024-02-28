using Florists.Core.Entities;

namespace Florists.Core.DTO.Flowers
{
  public record FlowersResultDTO(
    string Message,
    List<Flower> Flowers);
}
