using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Products;
using Florists.Core.Entities;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.Products.Commands.ProduceProduct
{
  public class ProduceProductCommandHandler : IRequestHandler<ProduceProductCommand, ErrorOr<ProduceProductResultDTO>>
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

    public async Task<ErrorOr<ProduceProductResultDTO>> Handle(
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

      var quantityBefore = productToProduce.AvailableQuantity;
      var quantityAfter = productToProduce.AvailableQuantity + command.QuantityToProduce;

      productToProduce.AvailableQuantity = quantityAfter;
      productToProduce.UpdatedAt = _dateTimeService.UtcNow;

      var productTransaction = new ProductTransaction
      {
        ProductTransactionId = Guid.NewGuid(),
        ProductId = productToProduce.ProductId,
        UserId = user.UserId,
        ProductionOrderNumber = command.ProductionOrderNumber,
        QuantityBefore = quantityBefore,
        QuantityAfter = quantityAfter,
        TransactionValue = productToProduce.UnitPrice * command.QuantityToProduce,
        TransactionType = ProductTransactionTypeOptions.ProduceProduct,
        CreatedAt = _dateTimeService.UtcNow,
        Product = productToProduce,
      };



      var inventoryTransactions = new List<InventoryTransaction>();

      foreach (var productInventory in productToProduce.ProductInventories)
      {
        if (productInventory.Inventory!.AvailableQuantity < productInventory.RequiredQuantity * command.QuantityToProduce)
        {
          return CustomErrors.Inventories.InsufficientQuantity(productInventory.Inventory.InventoryName);
        }

        var inventoryQuantityBefore = productInventory.Inventory.AvailableQuantity;
        var inventoryQuantityAfter = productInventory.Inventory.AvailableQuantity - (productInventory.RequiredQuantity * command.QuantityToProduce);
        productInventory.Inventory.AvailableQuantity = quantityAfter;
        productInventory.Inventory.UpdatedAt = _dateTimeService.UtcNow;


        var inventoryTransaction = new InventoryTransaction
        {
          InventoryTransactionId = Guid.NewGuid(),
          InventoryId = productInventory.InventoryId,
          UserId = user.UserId,
          ProductionOrderNumber = command.ProductionOrderNumber,
          QuantityBefore = inventoryQuantityBefore,
          QuantityAfter = inventoryQuantityAfter,
          TransactionValue = productInventory.Inventory.UnitPrice * (productInventory.RequiredQuantity * command.QuantityToProduce),
          TransactionType = InventoryTransactionTypeOptions.ProduceProduct,
          CreatedAt = _dateTimeService.UtcNow,
          Inventory = productInventory.Inventory
        };

        inventoryTransactions.Add(inventoryTransaction);
      }

      var success = await _productRepository.ProduceAsync(productTransaction, inventoryTransactions);

      if (!success)
      {
        return CustomErrors.Database.SaveError;
      }

      return new ProduceProductResultDTO(
        Messages.Database.SaveSuccess,
        productTransaction,
        inventoryTransactions);
    }
  }
}
