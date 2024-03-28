using Florists.Application.Features.Products.Commands.CreateProduct;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.DTO.Flowers;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Products.Commands.TestUtils
{
  public static class CreateProductCommandUtils
  {
    public static CreateProductCommand CreateCommand(int inventoriesCount = 3)
    {
      return new CreateProductCommand(
        Constants.Products.ProductName,
        Constants.Products.UnitPrice,
        Constants.Products.Sku,
        Constants.Products.Category,
        CreateRequiredInventories(inventoriesCount));
    }

    public static List<RequiredInventoryDTO> CreateRequiredInventories(int inventoriesCount)
    {
      var requiredInventories = new List<RequiredInventoryDTO>();

      for (int i = 0; i < inventoriesCount; i++)
      {
        var requiredInventory = new RequiredInventoryDTO(
          Guid.Parse(Constants.Products.InventoryId),
          i + 1);

        requiredInventories.Add(requiredInventory);
      }

      return requiredInventories;
    }

    public static Product CreateProduct(int inventoriesCount = 3)
    {
      return new Product
      {
        ProductId = Guid.Parse(Constants.Products.ProductId),
        ProductName = Constants.Products.ProductName,
        AvailableQuantity = Constants.Products.AvailableQuantity,
        UnitPrice = Constants.Products.UnitPrice,
        Sku = Constants.Products.Sku,
        IsActive = true,
        Category = Constants.Products.Category,
        CreatedAt = DateTime.Parse(Constants.Products.UtcNow),
        UpdatedAt = DateTime.Parse(Constants.Products.UtcNow),
        ProductInventories = CreateProductInventories(inventoriesCount)
      };
    }
    public static List<ProductInventory> CreateProductInventories(int inventoriesCount)
    {
      var productInventories = new List<ProductInventory>(inventoriesCount);

      for (int i = 0; i < inventoriesCount; i++)
      {
        var productInventory = new ProductInventory
        {
          ProductId = Guid.Parse(Constants.Products.ProductId),
          InventoryId = Guid.Parse(Constants.Products.InventoryId),
          RequiredQuantity = i,
          Inventory = new Inventory
          {
            InventoryId = Guid.Parse(Constants.Products.InventoryId),
            InventoryName = Constants.Products.InventorytNameFromIndex(i),
            AvailableQuantity = Constants.Products.InventoryAvailableQuantity,
            UnitPrice = Constants.Products.InventoryUnitPrice,
            Category = Constants.Products.InventoryCategory,
            CreatedAt = DateTime.Parse(Constants.Products.UtcNow),
            UpdatedAt = DateTime.Parse(Constants.Products.UtcNow),
          }
        };

        productInventories.Add(productInventory);
      }

      return productInventories;
    }
  }
}
