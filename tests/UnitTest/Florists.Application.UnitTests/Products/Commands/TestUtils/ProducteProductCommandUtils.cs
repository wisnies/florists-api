using Florists.Application.Features.Products.Commands.ProduceProduct;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Products.Commands.TestUtils
{
  public static class ProducteProductCommandUtils
  {
    public static ProduceProductCommand CreateCommand(int count = 1)
    {
      return new ProduceProductCommand(
        Guid.Parse(Constants.Products.ProductId),
        count,
        Constants.Products.ProductionOrderNumber,
        Constants.Products.Email);
    }

    public static FloristsUser CreateUser()
    {
      return new FloristsUser
      {
        UserId = Guid.Parse(Constants.Products.UserId),
        Email = Constants.Products.Email,
        FirstName = Constants.Products.FirstName,
        LastName = Constants.Products.LastName,
        CreatedAt = DateTime.Parse(Constants.Products.UtcNow),
        Role = new FloristsRole
        {
          RoleId = Guid.Parse(Constants.Products.RoleId),
          UserId = Guid.Parse(Constants.Products.UserId),
          RoleType = Constants.Products.RoleType,
          CreatedAt = DateTime.Parse(Constants.Products.UtcNow),
        }
      };
    }

    public static Product CreateProduct(int inventoriesCount = 1, bool withoutInventories = false, bool withInsufficientInventories = false)
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
        ProductInventories = withoutInventories ? null : CreateProductInventories(inventoriesCount, withInsufficientInventories)
      };
    }
    public static List<ProductInventory> CreateProductInventories(int inventoriesCount, bool withInsufficientInventories)
    {
      var productInventories = new List<ProductInventory>(inventoriesCount);

      for (int i = 0; i < inventoriesCount; i++)
      {
        var productInventory = new ProductInventory
        {
          ProductId = Guid.Parse(Constants.Products.ProductId),
          InventoryId = Guid.Parse(Constants.Products.InventoryIdFromIndex(i)),
          RequiredQuantity = i,
          Inventory = new Inventory
          {
            InventoryId = Guid.Parse(Constants.Products.InventoryIdFromIndex(i)),
            InventoryName = Constants.Products.InventorytNameFromIndex(i),
            AvailableQuantity = withInsufficientInventories ? -1 : 999999,
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
