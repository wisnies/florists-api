using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Common;
using Florists.Core.Entities;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.Products.Commands.ProduceProduct
{
  public class ProduceProductCommandHandler : IRequestHandler<ProduceProductCommand, ErrorOr<MessageResultDTO>>
  {
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeService _dateTimeService;

    public ProduceProductCommandHandler(
      IProductRepository productRepository,
      IUserRepository userRepository,
      IDateTimeService dateTimeService)
    {
      _productRepository = productRepository;
      _userRepository = userRepository;
      _dateTimeService = dateTimeService;
    }

    public async Task<ErrorOr<MessageResultDTO>> Handle(
      ProduceProductCommand command,
      CancellationToken cancellationToken)
    {
      var user = await _userRepository.GetOneByEmailAsync(command.Email);

      if (user is null)
      {
        return CustomErrors.Users.NotFound;
      }

      var productToProduce = await _productRepository.GetOneByIdAsync(command.ProductId, true);

      if (productToProduce is null)
      {
        return CustomErrors.Products.NotFound;
      }

      if (productToProduce.ProductInventories is null ||
        productToProduce.ProductInventories.Any(x => x.Inventory is null))
      {
        return CustomErrors.Database.FetchError;
      }

      var productTransaction = new ProductTransaction
      {
        ProductTransactionId = Guid.NewGuid(),
        ProductId = productToProduce.ProductId,
        UserId = user.UserId,
        ProductionOrderNumber = command.ProductionOrderNumber,
        QuantityBefore = productToProduce.AvailableQuantity,
        QuantityAfter = productToProduce.AvailableQuantity + command.QuantityToProduce,
        TransactionValue = productToProduce.UnitPrice * command.QuantityToProduce,
        TransactionType = ProductTransactionTypeOptions.ProduceProduct,
        CreatedAt = _dateTimeService.UtcNow,
        Product = productToProduce,
      };

      productToProduce.AvailableQuantity += command.QuantityToProduce;
      productToProduce.UpdatedAt = _dateTimeService.UtcNow;

      var inventoryTransactions = new List<InventoryTransaction>();

      foreach (var productInventory in productToProduce.ProductInventories)
      {
        if (productInventory.Inventory!.AvailableQuantity < productInventory.RequiredQuantity * command.QuantityToProduce)
        {
          return CustomErrors.Inventories.InsufficientQuantity(productInventory.Inventory.InventoryName);
        }

        var inventoryTransaction = new InventoryTransaction
        {
          InventoryTransactionId = Guid.NewGuid(),
          InventoryId = productInventory.InventoryId,
          UserId = user.UserId,
          ProductionOrderNumber = command.ProductionOrderNumber,
          QuantityBefore = productInventory.Inventory.AvailableQuantity,
          QuantityAfter = productInventory.Inventory.AvailableQuantity - (productInventory.RequiredQuantity * command.QuantityToProduce),
          TransactionValue = productInventory.Inventory.UnitPrice * (productInventory.RequiredQuantity * command.QuantityToProduce),
          TransactionType = InventoryTransactionTypeOptions.ProduceProduct,
          CreatedAt = _dateTimeService.UtcNow,
          Inventory = productInventory.Inventory
        };

        productInventory.Inventory.AvailableQuantity -= (productInventory.RequiredQuantity * command.QuantityToProduce);
        productInventory.Inventory.UpdatedAt = _dateTimeService.UtcNow;

        inventoryTransactions.Add(inventoryTransaction);
      }

      var success = await _productRepository.ProduceAsync(productTransaction, inventoryTransactions);

      if (!success)
      {
        return CustomErrors.Database.SaveError;
      }

      return new MessageResultDTO(
        true,
        Messages.Database.SaveSuccess);
    }
  }
}
