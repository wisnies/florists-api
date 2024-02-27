using Dapper;
using Florists.Infrastructure.Interfaces;
using Florists.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Florists.Infrastructure
{
  public class DataAccess : IDataAccess
  {
    private readonly MySqlSettings _settings;

    public DataAccess(IOptions<MySqlSettings> mySqlSettings)
    {
      _settings = mySqlSettings.Value;
    }

    public async Task<List<T>> LoadData<T, U>(
  string sql,
  U parameters)
    {
      using IDbConnection connection =
        new MySqlConnection(_settings.ConnectionString);
      var rows = await connection.QueryAsync<T>(sql, parameters);
      return rows.ToList();
    }

    public Task<int> SaveData<T>(
      string sql,
      T parameters)
    {
      using IDbConnection connection =
        new MySqlConnection(_settings.ConnectionString);
      return connection.ExecuteAsync(sql, parameters);
    }
  }
}
