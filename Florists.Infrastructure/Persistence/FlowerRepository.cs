using Florists.Application.Interfaces.Persistence;
using Florists.Core.Entities;
using Florists.Infrastructure.Interfaces;
using Florists.Infrastructure.Records;
using Florists.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Florists.Infrastructure.Persistence
{
  public class FlowerRepository : IFlowerRepository
  {
    private readonly MySqlSettings _settings;
    private readonly IDataAccess _dataAccess;

    public FlowerRepository(
      IOptions<MySqlSettings> mySqlOptions,
      IDataAccess dataAccess)
    {
      _settings = mySqlOptions.Value;
      _dataAccess = dataAccess;
    }

    public async Task<bool> CreateAsync(Flower flower)
    {
      try
      {
        var sql = $"INSERT INTO {_settings.FlowersTable} " +
          $"(flower_id, flower_name, available_quantity, unit_price, created_at) " +
          $"VALUES (@FlowerId, @FlowerName, @AvailableQuantity, @UnitPrice, @CreatedAt)";

        var parameters = new
        {
          flower.FlowerId,
          flower.FlowerName,
          flower.AvailableQuantity,
          flower.UnitPrice,
          flower.CreatedAt
        };

        var rowsAffected = await _dataAccess.SaveData(sql, parameters);

        return rowsAffected > 0;
      }
      catch
      {
        return false;
      }
    }

    public async Task<List<Flower>?> GetManyByNameAsync(string flowerName, int offset, int perPage)
    {
      try
      {
        var sql = $"SELECT * FROM {_settings.FlowersTable} WHERE flower_name LIKE @FlowerName LIMIT @PerPage OFFSET @Offset";

        var parameters = new
        {
          FlowerName = $"%{flowerName}%",
          PerPage = perPage,
          Offset = offset,
        };

        var result = await _dataAccess.LoadData<FlowerRecord, dynamic>(sql, parameters);

        List<Flower> flowers = result.ConvertAll(x => new Flower
        {
          FlowerId = x.flower_id,
          FlowerName = x.flower_name,
          AvailableQuantity = x.available_quantity,
          UnitPrice = x.unit_price,
          CreatedAt = x.created_at,
          UpdatedAt = x.updated_at
        });

        return flowers;
      }
      catch
      {
        return null;
      }
    }

    public async Task<Flower?> GetOneByIdAsync(Guid flowerId)
    {
      try
      {
        var sql = $"SELECT * FROM {_settings.FlowersTable} WHERE flower_id = @FlowerId";

        var parameters = new { FlowerId = flowerId };

        var result = await _dataAccess.LoadData<FlowerRecord, dynamic>(sql, parameters);

        Flower? flower = result.Select(x => new Flower
        {
          FlowerId = x.flower_id,
          FlowerName = x.flower_name,
          AvailableQuantity = x.available_quantity,
          UnitPrice = x.unit_price,
          CreatedAt = x.created_at,
          UpdatedAt = x.updated_at
        })
          .FirstOrDefault();

        return flower;
      }
      catch
      {
        return null;
      }
    }

    public async Task<Flower?> GetOneByNameAsync(string flowerName)
    {
      try
      {
        var sql = $"SELECT * FROM {_settings.FlowersTable} WHERE flower_name = @FlowerName";

        var parameters = new { FlowerName = flowerName };

        var result = await _dataAccess.LoadData<FlowerRecord, dynamic>(sql, parameters);

        Flower? flower = result.Select(x => new Flower
        {
          FlowerId = x.flower_id,
          FlowerName = x.flower_name,
          AvailableQuantity = x.available_quantity,
          UnitPrice = x.unit_price,
          CreatedAt = x.created_at,
          UpdatedAt = x.updated_at
        })
          .FirstOrDefault();

        return flower;
      }
      catch
      {
        return null;
      }
    }

    public async Task<bool> UpdateAsync(Flower flower)
    {
      try
      {
        string sql = $"Update {_settings.FlowersTable} " +
      $"SET flower_name = @FlowerName, unit_price = @UnitPrice, available_quantity = @AvailableQuantity, updated_at = @UpdatedAt " +
      $"WHERE flower_id = @FlowerId";

        var parameters = new
        {
          flower.FlowerId,
          flower.FlowerName,
          flower.UnitPrice,
          flower.AvailableQuantity,
          flower.UpdatedAt
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
