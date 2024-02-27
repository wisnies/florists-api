using Florists.Application.Interfaces.Persistence;
using Florists.Core.Entities;
using Florists.Infrastructure.Interfaces;
using Florists.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Florists.Infrastructure.Persistence
{
  public class RoleRepository : IRoleRepository
  {
    private readonly MySqlSettings _settings;
    private readonly IDataAccess _dataAccess;

    public RoleRepository(
      IOptions<MySqlSettings> mySqlOptions,
      IDataAccess dataAccess)
    {
      _settings = mySqlOptions.Value;
      _dataAccess = dataAccess;
    }

    public async Task<bool> CreateAsync(FloristsRole role)
    {
      try
      {
        var sql = $"INSERT INTO {_settings.RolesTable} " +
          $"(role_id, user_id, role_type, created_at) " +
          $"VALUES (@RoleId, @UserId, @RoleType, @CreatedAt)";

        var parameters = new
        {
          role.RoleId,
          role.UserId,
          RoleTYpe = role.RoleType,
          role.CreatedAt
        };

        var rowsAffected = await _dataAccess.SaveData(sql, parameters);

        return rowsAffected > 0;
      }
      catch
      {
        return false;
      }
    }

    public async Task<bool> UpdateAsync(FloristsRole role)
    {
      try
      {
        string sql = $"Update {_settings.RolesTable} " +
      $"SET role_type = @RoleType " +
      $"WHERE user_id = @UserId AND role_id = @RoleId";

        var parameters = new
        {
          RoleId = role.RoleId,
          UserId = role.UserId,
          RoleType = role.RoleType,
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
