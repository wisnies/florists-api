using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Inventories;
using Florists.Core.Entities;
using MediatR;

namespace Florists.Application.Features.Inventories.Commands.CreateInventory
{
  public class CreateInventoryCommandHandler : IRequestHandler<CreateInventoryCommand, ErrorOr<InventoryResultDTO>>
  {
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IDateTimeService _dateTimeService;

    public CreateInventoryCommandHandler(
      IInventoryRepository inventoryRepository,
      IDateTimeService dateTimeService)
    {
      _inventoryRepository = inventoryRepository;
      _dateTimeService = dateTimeService;
    }

    public async Task<ErrorOr<InventoryResultDTO>> Handle(
      CreateInventoryCommand command,
      CancellationToken cancellationToken)
    {
      var dbInventory = await _inventoryRepository.GetOneByNameAsync(command.InventoryName);

      if (dbInventory is not null)
      {
        return CustomErrors.Inventories.AlreadyExists;
      }

      var inventory = new Inventory
      {
        InventoryId = Guid.NewGuid(),
        InventoryName = command.InventoryName,
        AvailableQuantity = command.AvailableQuantity,
        UnitPrice = command.UnitPrice,
        Category = command.Category,
        CreatedAt = _dateTimeService.UtcNow,
      };

      var success = await _inventoryRepository.CreateAsync(inventory);

      if (!success)
      {
        return CustomErrors.Database.SaveError;
      }

      return new InventoryResultDTO(
        Messages.Database.SaveSuccess,
        inventory);
    }
  }
}
