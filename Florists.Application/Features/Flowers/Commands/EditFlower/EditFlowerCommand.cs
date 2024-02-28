using ErrorOr;
using Florists.Core.DTO.Common;
using MediatR;

namespace Florists.Application.Features.Flowers.Commands.EditFlower
{
  public record EditFlowerCommand(
    Guid FlowerId,
    string FlowerName,
    int AvailableQuantity,
    double UnitPrice) : IRequest<ErrorOr<MessageResultDTO>>;
}
