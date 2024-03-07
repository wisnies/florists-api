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

    public async Task<int> CountByNameAsync(string productName)
    {
      try
      {
        var sql = $"SELECT COUNT(product_id) as Count FROM {_settings.ProductsTable} WHERE product_name LIKE @ProductName AND is_active = true";

        var parameters = new
        {
          ProductName = $"%{productName}%",
        };

        var result = await _dataAccess.LoadData<RecordCountDTO, dynamic>(sql, parameters);

        return result[0].Count;
      }
      catch
      {
        return 0;
      }
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

    public async Task<List<Product>?> GetManyByNameAsync(string productName, int offset, int perPage)
    {
      try
      {
        var sql = $"SELECT * FROM {_settings.ProductsTable} WHERE product_name LIKE @ProductName AND is_active = true LIMIT @PerPage OFFSET @Offset";

        var parameters = new
        {
          ProductName = $"%{productName}%",
          PerPage = perPage,
          Offset = offset,
        };

        var result = await _dataAccess.LoadData<ProductRecordDTO, dynamic>(sql, parameters);

        List<Product> products = result.ConvertAll(x => new Product
        {
          ProductId = x.product_id,
          ProductName = x.product_name,
          AvailableQuantity = x.available_quantity,
          UnitPrice = x.unit_price,
          Sku = x.sku,
          Category = x.category,
          CreatedAt = x.created_at,
          UpdatedAt = x.updated_at
        });

        return products;
      }
      catch
      {
        return null;
      }
    }

    public async Task<Product?> GetOneByIdAsync(Guid productId, bool withInventories = false)
    {
      try
      {
        Product? product = null;
        var parameters = new { ProductId = productId };

        if (withInventories == true)
        {
          var sql = $"SELECT p.product_id, p.product_name, p.available_quantity AS product_available_quantity, p.unit_price AS product_unit_price, p.sku, p.category as product_category, " +
            $"p.created_at as product_created_at, p.updated_at as product_updated_at, " +
            $"i.inventory_id, i.inventory_name, i.available_Quantity AS inventory_available_quantity, i.unit_price AS inventory_unit_price, i.category as inventory_category, " +
            $"i.created_at as inventory_created_at, i.updated_at as inventory_updated_at, pi.required_quantity " +
            $"FROM products as p, inventories as i, product_inventories as pi " +
            $"WHERE p.product_id = pi.product_id AND i.inventory_id = pi.inventory_id AND p.is_active = true AND p.product_id = @ProductId";

          var result = await _dataAccess.LoadData<ProductRecordWithInventoryDTO, dynamic>(sql, parameters);

          product = result.Select(x => new Product
          {
            ProductId = x.product_id,
            ProductName = x.product_name,
            AvailableQuantity = x.product_available_quantity,
            UnitPrice = x.product_unit_price,
            Sku = x.sku,
            IsActive = x.is_active,
            Category = x.category,
            CreatedAt = x.product_created_at,
            UpdatedAt = x.product_updated_at,
          }).FirstOrDefault();

          if (product is not null)
          {
            var productInventories = result.ConvertAll(x => new ProductInventory
            {
              ProductId = x.product_id,
              InventoryId = x.inventory_id,
              RequiredQuantity = x.required_quantity,
              Inventory = new Inventory
              {
                InventoryId = x.inventory_id,
                InventoryName = x.inventory_name,
                AvailableQuantity = x.inventory_available_quantity,
                UnitPrice = x.inventory_unit_price,
                Category = x.inventory_category,
                CreatedAt = x.inventory_created_at,
                UpdatedAt = x.inventory_updated_at,
              }
            });
            product.ProductInventories = productInventories;
          }
        }
        else
        {
          var sql = $"SELECT * FROM {_settings.ProductsTable} WHERE product_id = @ProductId AND is_active = true";
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

    public async Task<Product?> GetOneByNameAsync(string productName, bool withInventories = false)
    {
      try
      {
        Product? product = null;
        if (withInventories == true)
        {
        }
        else
        {
          var sql = $"SELECT * FROM {_settings.ProductsTable} WHERE product_name = @ProductName AND is_active = true";
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

    public async Task<bool> SoftDeleteAsync(Product productToDelete)
    {
      try
      {

        var sql = $"UPDATE {_settings.ProductsTable} " +
          $"SET is_active = false, updated_at = @UpdatedAt " +
          $"WHERE product_id = @ProductId";
        var parameters = new
        {
          productToDelete.ProductId,
          productToDelete.UpdatedAt
        };

        var rowsAffectd = await _dataAccess.SaveData(sql, parameters);

        return rowsAffectd > 0;
      }
      catch
      {
        return false;
      }
    }
  }
}
