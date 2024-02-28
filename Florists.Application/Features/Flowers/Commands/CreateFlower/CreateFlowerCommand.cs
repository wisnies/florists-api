using ErrorOr;
using Florists.Core.DTO.Common;
using MediatR;

namespace Florists.Application.Features.Flowers.Commands.CreateFlower
{
  public record CreateFlowerCommand(
    string FlowerName,
    int AvailableQuantity,
    double UnitPrice) : IRequest<ErrorOr<MessageResultDTO>>;
}
