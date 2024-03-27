using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.InventoryTransactions;
using Florists.Core.Entities;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.Inventories.Commands.PurchaseInventories
{
  public class PurchaseInventoriesCommandHandler : IRequestHandler<PurchaseInventoriesCommand, ErrorOr<InventoryTransactionsResultDTO>>
  {
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeService _dateTimeService;

    public PurchaseInventoriesCommandHandler(
      IInventoryRepository inventoryRepository,
      IDateTimeService dateTimeService,
      IUserRepository userRepository,
      IInventoryTransactionRepository inventoryTransactionRepository)
    {
      _inventoryRepository = inventoryRepository;
      _dateTimeService = dateTimeService;
      _userRepository = userRepository;
      _inventoryTransactionRepository = inventoryTransactionRepository;
    }

    public async Task<ErrorOr<InventoryTransactionsResultDTO>> Handle(
      PurchaseInventoriesCommand command,
      CancellationToken cancellationToken)
    {
      var user = await _userRepository.GetOneByEmailAsync(command.Email);

      if (user is null)
      {
        return CustomErrors.Users.NotFound;
      }

      var transactions = new List<InventoryTransaction>();

      foreach (var dto in command.InventoriesToPurchase)
      {
        var inventory = await _inventoryRepository.GetOneByIdAsync(dto.InventoryId);

        if (inventory is null)
        {
          return CustomErrors.Inventories.NotFound;
        }

        int quantityBefore = inventory.AvailableQuantity;
        int quantityAfter = inventory.AvailableQuantity + dto.QuantityToPurchase;

        inventory.AvailableQuantity = quantityAfter;
        inventory.UpdatedAt = _dateTimeService.UtcNow;

        var transaction = new InventoryTransaction
        {
          InventoryTransactionId = Guid.NewGuid(),
          InventoryId = inventory.InventoryId,
          UserId = user.UserId,
          PurchaseOrderNumber = command.PurchaseOrderNumber,
          QuantityBefore = quantityBefore,
          QuantityAfter = quantityAfter,
          TransactionValue = inventory.UnitPrice * dto.QuantityToPurchase,
          TransactionType = InventoryTransactionTypeOptions.PurchaseInventory,
          CreatedAt = _dateTimeService.UtcNow,
          Inventory = inventory
        };

        transactions.Add(transaction);
      }

      var success = await _inventoryTransactionRepository.PurchaseAsync(transactions);

      if (!success)
      {
        return CustomErrors.Database.SaveError;
      }

      return new InventoryTransactionsResultDTO(
        Messages.Database.SaveSuccess,
        transactions.Count,
        transactions);
    }
  }
}
