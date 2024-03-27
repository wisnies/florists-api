using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Inventories;
using MediatR;

namespace Florists.Application.Features.Inventories.Commands.EditInventory
{
  public class EditInventoryCommandHandler : IRequestHandler<EditInventoryCommand, ErrorOr<InventoryResultDTO>>
  {
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IDateTimeService _dateTimeService;

    public EditInventoryCommandHandler(
      IInventoryRepository inventoryRepository,
      IDateTimeService dateTimeService)
    {
      _inventoryRepository = inventoryRepository;
      _dateTimeService = dateTimeService;
    }

    public async Task<ErrorOr<InventoryResultDTO>> Handle(
      EditInventoryCommand command,
      CancellationToken cancellationToken)
    {
      var inventoryToUpdate = await _inventoryRepository.GetOneByIdAsync(command.InventoryId);

      if (inventoryToUpdate is null)
      {
        return CustomErrors.Inventories.NotFound;
      }

      var sameNameInventory = await _inventoryRepository.GetOneByNameAsync(command.InventoryName);

      if (sameNameInventory is not null && sameNameInventory.InventoryId != command.InventoryId)
      {
        return CustomErrors.Inventories.AlreadyExists;
      }

      inventoryToUpdate.InventoryName = command.InventoryName;
      inventoryToUpdate.UnitPrice = command.UnitPrice;
      inventoryToUpdate.AvailableQuantity = command.AvailableQuantity;
      inventoryToUpdate.Category = command.Category;
      inventoryToUpdate.UpdatedAt = _dateTimeService.UtcNow;

      var success = await _inventoryRepository.UpdateAsync(inventoryToUpdate);

      if (!success)
      {
        return CustomErrors.Database.SaveError;
      }

      return new InventoryResultDTO(
        Messages.Database.SaveSuccess,
        inventoryToUpdate);
    }
  }
}
