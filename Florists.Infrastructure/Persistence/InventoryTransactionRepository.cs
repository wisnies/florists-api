using Florists.Application.Interfaces.Persistence;
using Florists.Core.Entities;
using Florists.Core.Enums;
using Florists.Infrastructure.DTO.Common;
using Florists.Infrastructure.DTO.InventoryTransactions;
using Florists.Infrastructure.Interfaces;
using Florists.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Florists.Infrastructure.Persistence
{
  public class InventoryTransactionRepository : IInventoryTransactionRepository
  {
    private readonly MySqlSettings _settings;
    private readonly IDataAccess _dataAccess;

    public InventoryTransactionRepository(
      IOptions<MySqlSettings> mySqlOptions,
      IDataAccess dataAccess)
    {
      _settings = mySqlOptions.Value;
      _dataAccess = dataAccess;
    }

    public async Task<int> CountAsync(
      DateTime? dateFrom,
      DateTime? dateTo,
      InventoryTransactionTypeOptions? transactionType)
    {
      try
      {
        var sql = $"SELECT COUNT(transaction_id) as Count " +
          $"FROM {_settings.InventoryTransactionsTable} ";

        sql += AppendSearchParams(dateFrom, dateTo, transactionType);

        var parameters = new
        {
          DateFrom = dateFrom,
          DateTo = dateTo,
          TransactionType = transactionType
        };

        var result = await _dataAccess.LoadData<RecordCountDTO, dynamic>(sql, parameters);

        return result[0].Count;
      }
      catch
      {
        return 0;
      }
    }

    public async Task<List<InventoryTransaction>?> GetManyAsync(
      int offset,
      int perPage,
      DateTime? dateFrom,
      DateTime? dateTo,
      InventoryTransactionTypeOptions? transactionType,
      TransactionsOrderByOptions? orderBy,
      OrderOptions? order)
    {
      try
      {
        var sql = $"SELECT it.transaction_id, it.user_id, it.inventory_id, it.purchase_order_number, it.production_order_number, it.quantity_before, it.quantity_after, it.transaction_type, it.transaction_value, it.created_at, it.updated_at, " +
          $"u.first_name, u.last_name, " +
          $"i.inventory_name, i.unit_price, i.category " +
          $"FROM {_settings.InventoryTransactionsTable} it " +
          $"LEFT JOIN {_settings.UsersTable} u " +
          $"ON it.user_id = u.user_id " +
          $"LEFT JOIN {_settings.InventoriesTable} i " +
          $"ON it.inventory_id = i.inventory_id ";

        sql += AppendSearchParams(dateFrom, dateTo, transactionType);

        sql += AppendOrderingParams(orderBy, order);

        sql += AppendPaginationParams();

        var parameters = new
        {
          DateFrom = dateFrom,
          DateTo = dateTo,
          TransactionType = transactionType,
          PerPage = perPage,
          Offset = offset,
        };

        var result = await _dataAccess.LoadData<InventoryTransactionRecordWithUserAndInventoryDTO, dynamic>(sql, parameters);

        List<InventoryTransaction> transactions = result.ConvertAll(x => new InventoryTransaction
        {
          InventoryTransactionId = x.transaction_id,
          InventoryId = x.inventory_id,
          UserId = x.user_id,
          PurchaseOrderNumber = x.purchase_order_number,
          ProductionOrderNumber = x.production_order_number,
          QuantityBefore = x.quantity_before,
          QuantityAfter = x.quantity_after,
          TransactionValue = x.transaction_value,
          TransactionType = x.transaction_type,
          CreatedAt = x.created_at,
          UpdatedAt = x.updated_at,
          User = new FloristsUser
          {
            UserId = x.user_id,
            FirstName = x.first_name,
            LastName = x.last_name,
          },
          Inventory = new Inventory
          {
            InventoryId = x.inventory_id,
            InventoryName = x.inventory_name,
            UnitPrice = x.unit_price,
            Category = x.category,
          }
        });

        return transactions;
      }
      catch
      {
        return null;
      }
    }

    public async Task<bool> PurchaseAsync(List<InventoryTransaction> transactions)
    {
      try
      {
        var queries = new List<QueryDTO>();
        foreach (var transaction in transactions)
        {
          var createTransactionSql = $"INSERT INTO {_settings.InventoryTransactionsTable} " +
            $"(transaction_id, inventory_id, user_id, purchase_order_number, quantity_before, quantity_after, transaction_value, transaction_type, created_at) VALUES " +
            $"(@TransactionId, @InventoryId, @UserId, @PurchaseOrderNumber, @QuantityBefore, @QuantityAfter, @TransactionValue, @TransactionType, @CreatedAt)";

          var createTransactionParameters = new
          {
            TransactionId = transaction.InventoryTransactionId,
            transaction.InventoryId,
            transaction.UserId,
            transaction.PurchaseOrderNumber,
            transaction.QuantityBefore,
            transaction.QuantityAfter,
            transaction.TransactionValue,
            transaction.TransactionType,
            transaction.CreatedAt
          };

          var createTransactionQuery = new QueryDTO(
            createTransactionSql,
            createTransactionParameters);
          queries.Add(createTransactionQuery);

          var updateInventoriestSql = $"Update {_settings.InventoriesTable} " +
            $"SET available_quantity = @AvailableQuantity, updated_at = @UpdatedAt " +
            $"WHERE inventory_id = @InventoryId";

          var quantityToPurchase = transaction.QuantityAfter - transaction.QuantityBefore;

          var updateInventoriestParameters = new
          {
            transaction.InventoryId,
            AvailableQuantity = transaction.Inventory!.AvailableQuantity + quantityToPurchase,
            transaction.Inventory.UpdatedAt
          };

          var updateInventoriestQuery = new QueryDTO(
            updateInventoriestSql,
            updateInventoriestParameters);
          queries.Add(updateInventoriestQuery);
        }

        var rowsAffected = await _dataAccess.SaveTransactionData(queries);
        return rowsAffected > 0;
      }
      catch
      {
        return false;
      }
    }

    private static string AppendSearchParams(
      DateTime? dateFrom,
      DateTime? dateTo,
      InventoryTransactionTypeOptions? transactionType)
    {
      string sql = "";

      if (dateFrom is not null || dateTo is not null || transactionType is not null)
      {
        sql += "WHERE ";
      }

      if (dateFrom is not null)
      {
        sql += "created_at >= @DateFrom ";
      }

      if (dateTo is not null)
      {
        if (dateFrom is not null)
        {
          sql += "AND ";
        }
        sql += "created_at <= @DateTo ";
      }

      if (transactionType is not null)
      {
        if (dateTo is not null || dateFrom is not null)
        {
          sql += "AND ";
        }
        sql += "transaction_type = @TransactionType ";
      }

      return sql;
    }

    private static string AppendOrderingParams(
      TransactionsOrderByOptions? orderBy,
      OrderOptions? order)
    {
      string sql = "";
      if (orderBy is not null)
      {
        sql += $"ORDER BY {orderBy} ";

        if (order is not null)
        {
          if (order == OrderOptions.ASC)
          {
            sql += "ASC ";
          }
          else
          {
            sql += "DESC ";
          }
        }
      }
      return sql;
    }

    private static string AppendPaginationParams()
    {
      return "LIMIT @PerPage OFFSET @Offset";
    }
  }
}
