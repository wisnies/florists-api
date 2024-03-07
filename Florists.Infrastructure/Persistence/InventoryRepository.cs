using Florists.Application.Interfaces.Persistence;
using Florists.Core.Entities;
using Florists.Infrastructure.DTO.Common;
using Florists.Infrastructure.DTO.Inventories;
using Florists.Infrastructure.Interfaces;
using Florists.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Florists.Infrastructure.Persistence
{
  public class InventoryRepository : IInventoryRepository
  {
    private readonly MySqlSettings _settings;
    private readonly IDataAccess _dataAccess;

    public InventoryRepository(
      IOptions<MySqlSettings> mySqlOptions,
      IDataAccess dataAccess)
    {
      _settings = mySqlOptions.Value;
      _dataAccess = dataAccess;
    }

    public async Task<int> CountByNameAsync(string inventoryName)
    {
      try
      {
        var sql = $"SELECT COUNT(inventory_id) as Count FROM {_settings.InventoriesTable} WHERE inventory_name LIKE @InventoryName";

        var parameters = new
        {
          InventoryName = $"%{inventoryName}%",
        };

        var result = await _dataAccess.LoadData<RecordCountDTO, dynamic>(sql, parameters);

        return result[0].Count;
      }
      catch
      {
        return 0;
      }
    }

    public async Task<bool> CreateAsync(Inventory inventory)
    {
      try
      {
        var sql = $"INSERT INTO {_settings.InventoriesTable} " +
          $"(inventory_id, inventory_name, available_quantity, unit_price, category, created_at) " +
          $"VALUES (@InventoryId, @InventoryName, @AvailableQuantity, @UnitPrice, @Category, @CreatedAt)";

        var parameters = new
        {
          inventory.InventoryId,
          inventory.InventoryName,
          inventory.AvailableQuantity,
          inventory.UnitPrice,
          inventory.Category,
          inventory.CreatedAt
        };

        var rowsAffected = await _dataAccess.SaveData(sql, parameters);

        return rowsAffected > 0;
      }
      catch
      {
        return false;
      }
    }

    public async Task<List<Inventory>?> GetManyByNameAsync(string inventoryName, int offset, int perPage)
    {
      try
      {
        var sql = $"SELECT * FROM {_settings.InventoriesTable} WHERE inventory_name LIKE @InventoryName LIMIT @PerPage OFFSET @Offset";

        var parameters = new
        {
          InventoryName = $"%{inventoryName}%",
          PerPage = perPage,
          Offset = offset,
        };

        var result = await _dataAccess.LoadData<InventoryRecordDTO, dynamic>(sql, parameters);

        List<Inventory> inventories = result.ConvertAll(x => new Inventory
        {
          InventoryId = x.inventory_id,
          InventoryName = x.inventory_name,
          AvailableQuantity = x.available_quantity,
          UnitPrice = x.unit_price,
          Category = x.category,
          CreatedAt = x.created_at,
          UpdatedAt = x.updated_at
        });

        return inventories;
      }
      catch
      {
        return null;
      }
    }

    public async Task<Inventory?> GetOneByIdAsync(Guid inventoryId)
    {
      try
      {
        var sql = $"SELECT * FROM {_settings.InventoriesTable} WHERE inventory_id = @InventoryId";

        var parameters = new { InventoryId = inventoryId };

        var result = await _dataAccess.LoadData<InventoryRecordDTO, dynamic>(sql, parameters);

        Inventory? inventory = result.Select(x => new Inventory
        {
          InventoryId = x.inventory_id,
          InventoryName = x.inventory_name,
          AvailableQuantity = x.available_quantity,
          UnitPrice = x.unit_price,
          Category = x.category,
          CreatedAt = x.created_at,
          UpdatedAt = x.updated_at
        })
          .FirstOrDefault();

        return inventory;
      }
      catch
      {
        return null;
      }
    }

    public async Task<Inventory?> GetOneByNameAsync(string inventoryName)
    {
      try
      {
        var sql = $"SELECT * FROM {_settings.InventoriesTable} WHERE inventory_name = @InventoryName";

        var parameters = new { InventoryName = inventoryName };

        var result = await _dataAccess.LoadData<InventoryRecordDTO, dynamic>(sql, parameters);

        Inventory? inventory = result.Select(x => new Inventory
        {
          InventoryId = x.inventory_id,
          InventoryName = x.inventory_name,
          AvailableQuantity = x.available_quantity,
          UnitPrice = x.unit_price,
          Category = x.category,
          CreatedAt = x.created_at,
          UpdatedAt = x.updated_at
        })
          .FirstOrDefault();

        return inventory;
      }
      catch
      {
        return null;
      }
    }

    public async Task<bool> UpdateAsync(Inventory inventory)
    {
      try
      {
        string sql = $"Update {_settings.InventoriesTable} " +
      $"SET inventory_name = @InventoryName, unit_price = @UnitPrice, available_quantity = @AvailableQuantity, category = @Category, updated_at = @UpdatedAt " +
      $"WHERE inventory_id = @InventoryId";

        var parameters = new
        {
          inventory.InventoryId,
          inventory.InventoryName,
          inventory.UnitPrice,
          inventory.AvailableQuantity,
          inventory.Category,
          inventory.UpdatedAt
        };

        var rowsAffected = await _dataAccess.SaveData(sql, parameters);

        return rowsAffected > 0;
      }
      catch
      {
        return false;
      }
    }
  }
}
