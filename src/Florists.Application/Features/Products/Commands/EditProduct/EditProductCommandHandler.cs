using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Products;
using Florists.Core.Entities;
using MediatR;

namespace Florists.Application.Features.Products.Commands.EditProduct
{
  public class EditProductCommandHandler : IRequestHandler<EditProductCommand, ErrorOr<ProductResultDTO>>
  {
    private readonly IProductRepository _productRepository;
    private readonly IDateTimeService _dateTimeService;

    public EditProductCommandHandler(
      IProductRepository productRepository,
      IDateTimeService dateTimeService)
    {
      _productRepository = productRepository;
      _dateTimeService = dateTimeService;
    }

    public async Task<ErrorOr<ProductResultDTO>> Handle(
      EditProductCommand command,
      CancellationToken cancellationToken)
    {
      var productToUpdate = await _productRepository.GetOneByIdAsync(command.ProductId);

      if (productToUpdate is null)
      {
        return CustomErrors.Products.NotFound;
      }


      var productWithSameName = await _productRepository.GetOneByNameAsync(command.ProductName);

      if (productWithSameName is not null)
      {
        return CustomErrors.Products.AlreadyExists;
      }

      productToUpdate.ProductName = command.ProductName;
      productToUpdate.UnitPrice = command.UnitPrice;
      productToUpdate.Sku = command.Sku;
      productToUpdate.Category = command.Category;
      productToUpdate.UpdatedAt = _dateTimeService.UtcNow;

      var productInventories = new List<ProductInventory>();

      foreach (var requiredInventory in command.RequiredInventories)
      {
        var productInventory = new ProductInventory
        {
          ProductId = productToUpdate.ProductId,
          InventoryId = requiredInventory.InventoryId,
          RequiredQuantity = requiredInventory.RequiredQuantity,
          CreatedAt = productToUpdate.CreatedAt,
          UpdatedAt = _dateTimeService.UtcNow,
        };

        productInventories.Add(productInventory);
      }

      productToUpdate.ProductInventories = productInventories;

      var success = await _productRepository.UpdateAsync(productToUpdate);

      if (!success)
      {
        return CustomErrors.Database.SaveError;
      }

      return new ProductResultDTO(
        Messages.Database.UpdateSuccess,
        productToUpdate);
    }
  }
}
