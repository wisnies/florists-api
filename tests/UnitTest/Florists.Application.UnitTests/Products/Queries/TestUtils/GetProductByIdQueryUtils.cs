using Florists.Application.Features.Products.Queries.GetProductById;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Products.Queries.TestUtils
{
  public static class GetProductByNameQueryUtils
  {
    public static GetProductByIdQuery CreateQuery()
    {
      return new GetProductByIdQuery(Guid.Parse(Constants.Products.ProductId));
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
