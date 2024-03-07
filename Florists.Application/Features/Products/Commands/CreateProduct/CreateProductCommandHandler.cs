using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Products;
using Florists.Core.Entities;
using MediatR;

namespace Florists.Application.Features.Products.Commands.CreateProduct
{
  public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ErrorOr<ProductResultDTO>>
  {
    private readonly IProductRepository _productRepository;
    private readonly IDateTimeService _dateTimeService;

    public CreateProductCommandHandler(
      IProductRepository productRepository,
      IDateTimeService dateTimeService)
    {
      _productRepository = productRepository;
      _dateTimeService = dateTimeService;
    }

    public async Task<ErrorOr<ProductResultDTO>> Handle(
      CreateProductCommand command,
      CancellationToken cancellationToken)
    {
      var dbProduct = await _productRepository.GetOneByNameAsync(command.ProductName);

      if (dbProduct is not null)
      {
        return CustomErrors.Products.AlreadyExists;
      }

      var newProduct = new Product
      {
        ProductId = Guid.NewGuid(),
        ProductName = command.ProductName,
        UnitPrice = command.UnitPrice,
        Sku = command.Sku,
        Category = command.Category,
        CreatedAt = _dateTimeService.UtcNow
      };

      var productInventories = new List<ProductInventory>();

      foreach (var requiredInventory in command.RequiredInventories)
      {
        var productInventory = new ProductInventory
        {
          ProductId = newProduct.ProductId,
          InventoryId = requiredInventory.InventoryId,
          RequiredQuantity = requiredInventory.RequiredQuantity,
          CreatedAt = _dateTimeService.UtcNow
        };
        productInventories.Add(productInventory);
      }

      newProduct.ProductInventories = productInventories;

      var success = await _productRepository.CreateAsync(newProduct);

      if (!success)
      {
        return CustomErrors.Database.SaveError;
      }

      return new ProductResultDTO(
        Messages.Database.SaveSuccess,
        newProduct);
    }
  }
}
