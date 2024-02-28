using ErrorOr;
using Florists.Core.DTO.Common;
using Florists.Core.DTO.Flowers;
using MediatR;

namespace Florists.Application.Features.Flowers.Commands.PurchaseFlowers
{
  public record PurchaseFlowersCommand(
    string Email,
    string PurchaseOrderNumber,
    List<FlowerToPurchaseDTO> FlowersToPurchase) : IRequest<ErrorOr<MessageResultDTO>>;
}
