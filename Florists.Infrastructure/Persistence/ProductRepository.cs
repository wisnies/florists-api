using Florists.Application.Interfaces.Persistence;
using Florists.Core.Entities;
using Florists.Infrastructure.DTO.Common;
using Florists.Infrastructure.DTO.Products;
using Florists.Infrastructure.Interfaces;
using Florists.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Florists.Infrastructure.Persistence
{
  public class ProductRepository : IProductRepository
  {
    private readonly MySqlSettings _settings;
    private readonly IDataAccess _dataAccess;

    public ProductRepository(
      IOptions<MySqlSettings> mySqlOptions,
      IDataAccess dataAccess)
    {
      _settings = mySqlOptions.Value;
      _dataAccess = dataAccess;
    }

    public async Task<bool> CreateAsync(Product product)
    {
      var queries = new List<QueryDTO>();

      var createSql = $"INSERT INTO {_settings.ProductsTable} " +
        $"(product_id, product_name, available_quantity, unit_price, sku, is_active, category, created_at) " +
        $"VALUES " +
        $"(@ProductId, @ProductName, @AvailableQuantity, @UnitPrice, @Sku, @IsActive, @Category, @CreatedAt)";
      var createParameters = new
      {
        product.ProductId,
        product.ProductName,
        product.AvailableQuantity,
        product.UnitPrice,
        product.Sku,
        product.IsActive,
        product.Category,
        product.CreatedAt
      };

      var createQuery = new QueryDTO(createSql, createParameters);
      queries.Add(createQuery);

      if (product.ProductInventories is not null)
      {
        foreach (var productInventory in product.ProductInventories)
        {
          var createProductInventorySql = $"INSERT INTO {_settings.ProductInventoriesTable} " +
            $"(product_id, inventory_id, required_quantity, created_at) " +
            $"VALUES " +
            $"(@ProductId, @InventoryId, @RequiredQuantity, @CreatedAt)";
          var createProductInventoryParameters = new
          {
            productInventory.ProductId,
            productInventory.InventoryId,
            productInventory.RequiredQuantity,
            productInventory.CreatedAt
          };

          var createProductInventoryQuery = new QueryDTO(
            createProductInventorySql,
            createProductInventoryParameters);

          queries.Add(createProductInventoryQuery);
        }
      }

      var rowsAffected = await _dataAccess.SaveTransactionData(queries);

      return rowsAffected > 0;
    }

    public async Task<Product?> GetOneByNameAsync(string productName, bool withInventories = false)
    {
      try
      {
        Product? product = null;
        if (withInventories == true)
        {
          var sql = $"SELECT * FROM {_settings.ProductsTable} WHERE product_name = @ProductName";
          var parameters = new { ProductName = productName };

          var result = await _dataAccess.LoadData<ProductRecordDTO, dynamic>(sql, parameters);
        }
        else
        {
          var sql = $"SELECT * FROM {_settings.ProductsTable} WHERE product_name = @ProductName";
          var parameters = new { ProductName = productName };
          var result = await _dataAccess.LoadData<ProductRecordDTO, dynamic>(sql, parameters);
          product = result.Select(x => new Product
          {
            ProductId = x.product_id,
            ProductName = x.product_name,
            AvailableQuantity = x.available_quantity,
            UnitPrice = x.unit_price,
            Sku = x.sku,
            IsActive = x.is_active,
            Category = x.category,
            CreatedAt = x.created_at,
            UpdatedAt = x.updated_at,
          }).FirstOrDefault();
        }
        return product;
      }
      catch
      {
        return null;
      }
    }
  }
}
