using ErrorOr;
using Florists.Core.DTO.Common;
using Florists.Core.DTO.Flowers;
using MediatR;

namespace Florists.Application.Features.Inventories.Commands.PurchaseInventories
{
  public record PurchaseInventoriesCommand(
    string Email,
    string PurchaseOrderNumber,
    List<InventoryToPurchaseDTO> InventoriesToPurchase) : IRequest<ErrorOr<MessageResultDTO>>;
}
