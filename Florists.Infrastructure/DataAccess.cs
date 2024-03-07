using Dapper;
using Florists.Infrastructure.DTO.Common;
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

    public async Task<int> SaveTransactionData(List<QueryDTO> queries)
    {
      using (IDbConnection connection =
        new MySqlConnection(_settings.ConnectionString))
      {
        connection.Open();

        using (var transaction = connection.BeginTransaction())
        {
          try
          {
            var totalAffectedRows = 0;
            foreach (var query in queries)
            {
              var affectedRows = await connection.ExecuteAsync(query.Sql, query.Parameters);
              if (affectedRows == 0)
              {
                throw new Exception();
              }
              totalAffectedRows++;
            }

            transaction.Commit();
            return totalAffectedRows;
          }
          catch
          {
            transaction.Rollback();
            return 0;
          }
        }
      }
    }
  }
}
