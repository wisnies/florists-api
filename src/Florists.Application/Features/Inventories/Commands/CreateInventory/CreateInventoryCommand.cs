using ErrorOr;
using Florists.Core.DTO.Common;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.Inventories.Commands.CreateInventory
{
  public record CreateInventoryCommand(
    string InventoryName,
    int AvailableQuantity,
    double UnitPrice,
    InventoryCategoryOptions Category) : IRequest<ErrorOr<MessageResultDTO>>;
}
