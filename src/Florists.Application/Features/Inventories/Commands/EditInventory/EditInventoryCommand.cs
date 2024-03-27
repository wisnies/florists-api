using ErrorOr;
using Florists.Core.DTO.Inventories;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.Inventories.Commands.EditInventory
{
  public record EditInventoryCommand(
    Guid InventoryId,
    string InventoryName,
    int AvailableQuantity,
    double UnitPrice,
    InventoryCategoryOptions Category) : IRequest<ErrorOr<InventoryResultDTO>>;
}
