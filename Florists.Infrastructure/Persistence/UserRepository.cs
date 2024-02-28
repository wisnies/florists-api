using Florists.Application.Interfaces.Persistence;
using Florists.Core.DTO.Auth;
using Florists.Core.Entities;
using Florists.Infrastructure.Interfaces;
using Florists.Infrastructure.Records;
using Florists.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Florists.Infrastructure.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly MySqlSettings _settings;
    private readonly IDataAccess _dataAccess;

    public UserRepository(
      IOptions<MySqlSettings> mySqlOptions,
      IDataAccess dataAccess)
    {
      _settings = mySqlOptions.Value;
      _dataAccess = dataAccess;
    }
    public async Task<FloristsUser?> GetOneByEmailAsync(string email)
    {
      try
      {
        string sql = $"SELECT u.user_id, r.role_id, r.role_type, u.is_active, u.is_password_changed, u.email, u.first_name, u.last_name, u.password_hash, u.refresh_token, u.refresh_token_expiration, u.created_at AS user_created_at, u.updated_at AS user_updated_at, r.created_at AS role_created_at, r.updated_at AS role_updated_at " +
          $"FROM {_settings.UsersTable} u " +
          $"LEFT JOIN {_settings.RolesTable} r " +
          $"ON u.user_id = r.user_id " +
          $"WHERE u.email = @Email AND u.is_active = true";

        var parameters = new
        {
          Email = email
        };

        var result = await _dataAccess.LoadData<UserRecordWithRole, dynamic>(sql, parameters);

        FloristsUser? user = result.Select(x => new FloristsUser
        {
          UserId = x.user_id,
          IsActive = x.is_active,
          IsPasswordChanged = x.is_password_changed,
          Email = x.email,
          PasswordHash = x.password_hash,
          FirstName = x.first_name,
          LastName = x.last_name,
          RefreshToken = x.refresh_token,
          RefreshTokenExpiration = x.refresh_token_expiration,
          CreatedAt = x.user_created_at,
          UpdatedAt = x.user_updated_at,
          Role = new FloristsRole
          {
            RoleId = x.role_id,
            UserId = x.user_id,
            RoleType = x.role_type,
            CreatedAt = x.role_created_at,
            UpdatedAt = x.role_updated_at,
          }
        }).FirstOrDefault();

        return user;
      }
      catch
      {
        return null;
      }
    }

    public async Task<bool> AuthenticateAsync(FloristsUser user, UserTokensDTO tokenResult)
    {
      try
      {
        string sql = $"Update {_settings.UsersTable} " +
      $"SET refresh_token = @RefreshToken, refresh_token_expiration = @RefreshTokenExpiration " +
      $"WHERE user_id = @UserId AND is_active = true";

        var parameters = new
        {
          user.UserId,
          tokenResult.RefreshToken,
          tokenResult.RefreshTokenExpiration,
        };

        var rowsAffected = await _dataAccess.SaveData(sql, parameters);

        return rowsAffected > 0;
      }
      catch
      {
        return false;
      }
    }

    public async Task<FloristsUser?> GetOneByRefreshTokenAsync(string refreshToken)
    {
      try
      {
        string sql = $"SELECT u.user_id, r.role_id, r.role_type, u.is_active, u.is_password_changed, u.email, u.first_name, u.last_name, u.refresh_token, u.refresh_token_expiration, u.created_at AS user_created_at, u.updated_at AS user_updated_at, r.created_at AS role_created_at, r.updated_at AS role_updated_at " +
          $"FROM {_settings.UsersTable} u " +
          $"LEFT JOIN {_settings.RolesTable} r " +
          $"ON u.user_id = r.user_id " +
          $"WHERE u.refresh_token = @RefreshToken";

        var parameters = new
        {
          RefreshToken = refreshToken
        };

        var result = await _dataAccess.LoadData<UserRecordWithRole, dynamic>(sql, parameters);

        FloristsUser? user = result.Select(x => new FloristsUser
        {
          UserId = x.user_id,
          IsActive = x.is_active,
          IsPasswordChanged = x.is_password_changed,
          Email = x.email,
          FirstName = x.first_name,
          LastName = x.last_name,
          RefreshToken = x.refresh_token,
          RefreshTokenExpiration = x.refresh_token_expiration,
          CreatedAt = x.user_created_at,
          UpdatedAt = x.user_updated_at,
          Role = new FloristsRole
          {
            RoleId = x.role_id,
            UserId = x.user_id,
            RoleType = x.role_type,
            CreatedAt = x.role_created_at,
            UpdatedAt = x.role_updated_at,
          }
        }).FirstOrDefault();

        return user;
      }
      catch
      {
        return null;
      }
    }

    public async Task<bool> LogoutAsync(FloristsUser user)
    {
      try
      {
        string sql = $"Update {_settings.UsersTable} " +
      $"SET refresh_token = null, refresh_token_expiration = null " +
      $"WHERE user_id = @UserId AND is_active = true";

        var parameters = new
        {
          user.UserId,
        };

        var rowsAffected = await _dataAccess.SaveData(sql, parameters);

        return rowsAffected > 0;
      }
      catch
      {
        return false;
      }
    }

    public async Task<bool> ChangePasswordAsync(FloristsUser user)
    {
      try
      {
        string sql = $"Update {_settings.UsersTable} " +
      $"SET password_hash = @PasswordHash, is_password_changed = @IsPasswordChanged " +
      $"WHERE user_id = @UserId AND is_active = true";

        var parameters = new
        {
          user.UserId,
          user.PasswordHash,
          user.IsPasswordChanged,
        };

        var rowsAffected = await _dataAccess.SaveData(sql, parameters);

        return rowsAffected > 0;
      }
      catch
      {
        return false;
      }
    }

    public async Task<bool> CreateAsync(FloristsUser user)
    {
      try
      {
        var sql = $"INSERT INTO {_settings.UsersTable} " +
          $"(user_id, is_active, is_password_changed, password_hash, email, first_name, last_name, created_at) " +
          $"VALUES (@UserId, @IsActive, @IsPasswordChanged, @PasswordHash, @Email, @FirstName, @LastName, @CreatedAt)";

        var parameters = new
        {
          user.UserId,
          user.IsActive,
          user.IsPasswordChanged,
          user.PasswordHash,
          user.Email,
          user.FirstName,
          user.LastName,
          user.CreatedAt
        };

        var rowsAffected = await _dataAccess.SaveData(sql, parameters);

        return rowsAffected > 0;
      }
      catch
      {
        return false;
      }
    }

    public async Task<bool> Delete(FloristsUser user)
    {
      try
      {
        var sql = $"DELETE FROM {_settings.UsersTable} WHERE user_id = @UserId";

        var parameters = new
        {
          user.UserId,
        };

        var rowsAffected = await _dataAccess.SaveData(sql, parameters);

        return rowsAffected > 0;
      }
      catch
      {
        return false;
      }
    }

    public async Task<bool> DeleteSoft(FloristsUser user)
    {
      try
      {
        string sql = $"Update {_settings.UsersTable} " +
      $"SET is_active = false " +
      $"WHERE user_id = @UserId AND is_active = true";

        var parameters = new
        {
          user.UserId
        };

        var rowsAffected = await _dataAccess.SaveData(sql, parameters);

        return rowsAffected > 0;
      }
      catch
      {
        return false;
      }
    }

    public async Task<FloristsUser?> GetOneByIdAsync(Guid userId)
    {
      try
      {
        string sql = $"SELECT u.user_id, r.role_id, r.role_type, u.is_active, u.is_password_changed, u.email, u.first_name, u.last_name, u.password_hash, u.refresh_token, u.refresh_token_expiration, u.created_at AS user_created_at, u.updated_at AS user_updated_at, r.created_at AS role_created_at, r.updated_at AS role_updated_at " +
          $"FROM {_settings.UsersTable} u " +
          $"LEFT JOIN {_settings.RolesTable} r " +
          $"ON u.user_id = r.user_id " +
          $"WHERE u.user_id = @UserId AND u.is_active = true";

        var parameters = new
        {
          UserId = userId
        };

        var result = await _dataAccess.LoadData<UserRecordWithRole, dynamic>(sql, parameters);

        FloristsUser? user = result.Select(x => new FloristsUser
        {
          UserId = x.user_id,
          IsActive = x.is_active,
          IsPasswordChanged = x.is_password_changed,
          Email = x.email,
          PasswordHash = x.password_hash,
          FirstName = x.first_name,
          LastName = x.last_name,
          RefreshToken = x.refresh_token,
          RefreshTokenExpiration = x.refresh_token_expiration,
          CreatedAt = x.user_created_at,
          UpdatedAt = x.user_updated_at,
          Role = new FloristsRole
          {
            RoleId = x.role_id,
            UserId = x.user_id,
            RoleType = x.role_type,
            CreatedAt = x.role_created_at,
            UpdatedAt = x.role_updated_at,
          }
        }).FirstOrDefault();

        return user;
      }
      catch
      {
        return null;
      }
    }

    public async Task<List<FloristsUser>?> GetManyByLastNameAsync(string lastName, int offset, int perPage)
    {
      try
      {
        string sql = $"SELECT u.user_id, r.role_id, r.role_type, u.is_active, u.is_password_changed, u.email, u.first_name, u.last_name, u.password_hash, u.refresh_token, u.refresh_token_expiration, u.created_at AS user_created_at, u.updated_at AS user_updated_at, r.created_at AS role_created_at, r.updated_at AS role_updated_at " +
          $"FROM {_settings.UsersTable} u " +
          $"LEFT JOIN {_settings.RolesTable} r " +
          $"ON u.user_id = r.user_id " +
          $"WHERE u.last_name LIKE @LastName AND u.is_active = true " +
          $"LIMIT @PerPage " +
          $"OFFSET @Offset";

        var parameters = new
        {
          LastName = $"%{lastName}%",
          PerPage = perPage,
          Offset = offset,
        };

        var result = await _dataAccess.LoadData<UserRecordWithRole, dynamic>(sql, parameters);

        List<FloristsUser> users = result.ConvertAll(x => new FloristsUser
        {
          UserId = x.user_id,
          IsActive = x.is_active,
          IsPasswordChanged = x.is_password_changed,
          Email = x.email,
          PasswordHash = x.password_hash,
          FirstName = x.first_name,
          LastName = x.last_name,
          RefreshToken = x.refresh_token,
          RefreshTokenExpiration = x.refresh_token_expiration,
          CreatedAt = x.user_created_at,
          UpdatedAt = x.user_updated_at,
          Role = new FloristsRole
          {
            RoleId = x.role_id,
            UserId = x.user_id,
            RoleType = x.role_type,
            CreatedAt = x.role_created_at,
            UpdatedAt = x.role_updated_at,
          }
        });

        return users;
      }
      catch
      {
        return null;
      }
    }

    public async Task<bool> UpdateAsync(FloristsUser user)
    {
      try
      {
        string sql = $"Update {_settings.UsersTable} " +
      $"SET email = @Email, first_name = @FirstName, last_name = @LastName, updated_at = @UpdatedAt " +
      $"WHERE user_id = @UserId AND is_active = true";

        var parameters = new
        {
          user.UserId,
          user.Email,
          user.FirstName,
          user.LastName,
          user.UpdatedAt
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
