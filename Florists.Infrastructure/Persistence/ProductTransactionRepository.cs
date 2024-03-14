using Florists.Application.Interfaces.Persistence;
using Florists.Core.Entities;
using Florists.Core.Enums;
using Florists.Infrastructure.DTO.Common;
using Florists.Infrastructure.DTO.ProductTransactions;
using Florists.Infrastructure.Interfaces;
using Florists.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Florists.Infrastructure.Persistence
{
  public class ProductTransactionRepository : IProductTransactionRepository
  {
    private readonly MySqlSettings _settings;
    private readonly IDataAccess _dataAccess;

    public ProductTransactionRepository(
      IOptions<MySqlSettings> mySqlOptions,
      IDataAccess dataAccess)
    {
      _settings = mySqlOptions.Value;
      _dataAccess = dataAccess;
    }

    public async Task<int> CountAsync(DateTime? dateFrom, DateTime? dateTo, ProductTransactionTypeOptions? transactionType)
    {
      try
      {
        var sql = $"SELECT COUNT(transaction_id) as Count " +
          $"FROM {_settings.ProductTransactionsTable} ";

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

    public async Task<List<ProductTransaction>?> GetManyAsync(
      int offset,
      int perPage,
      DateTime? dateFrom,
      DateTime? dateTo,
      ProductTransactionTypeOptions? transactionType,
      TransactionsOrderByOptions? orderBy,
      OrderOptions? order)
    {
      try
      {
        var sql = $"SELECT pt.transaction_id, pt.user_id, pt.product_id, pt.sale_order_number, pt.production_order_number, pt.quantity_before, pt.quantity_after, pt.transaction_type, pt.transaction_value, pt.created_at, pt.updated_at, " +
          $"u.first_name, u.last_name, " +
          $"p.product_name, p.unit_price, p.category " +
          $"FROM {_settings.ProductTransactionsTable} pt " +
          $"LEFT JOIN {_settings.UsersTable} u " +
          $"ON pt.user_id = u.user_id " +
          $"LEFT JOIN {_settings.ProductsTable} p " +
          $"ON pt.product_id = p.product_id ";

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

        var result = await _dataAccess.LoadData<ProductTransactionRecordWithUserAndProductDTO, dynamic>(sql, parameters);

        List<ProductTransaction> transactions = result.ConvertAll(x => new ProductTransaction
        {
          ProductTransactionId = x.transaction_id,
          ProductId = x.product_id,
          UserId = x.user_id,
          SaleOrderNumber = x.sale_order_number,
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
          Product = new Product
          {
            ProductId = x.product_id,
            ProductName = x.product_name,
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

    public async Task<bool> SellAsync(List<ProductTransaction> transactions)
    {
      try
      {
        var queries = new List<QueryDTO>();
        foreach (var transaction in transactions)
        {
          if (transaction.Product is null)
          {
            return false;
          }

          var createTransactionSql = $"INSERT INTO {_settings.ProductTransactionsTable} " +
            $"(transaction_id, product_id, user_id, sale_order_number, quantity_before, quantity_after, transaction_value, transaction_type, created_at) VALUES " +
            $"(@TransactionId, @ProductId, @UserId, @SaleOrderNumber, @QuantityBefore, @QuantityAfter, @TransactionValue, @TransactionType, @CreatedAt)";

          var createTransactionParameters = new
          {
            TransactionId = transaction.ProductTransactionId,
            transaction.ProductId,
            transaction.UserId,
            transaction.SaleOrderNumber,
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

          var updateProductSql = $"Update {_settings.ProductsTable} " +
            $"SET available_quantity = @AvailableQuantity, updated_at = @UpdatedAt " +
            $"WHERE product_id = @ProductId";

          var updateProductParameters = new
          {
            transaction.ProductId,
            transaction.Product.AvailableQuantity,
            transaction.Product.UpdatedAt
          };

          var updateProductQuery = new QueryDTO(
            updateProductSql,
            updateProductParameters);
          queries.Add(updateProductQuery);
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
        ProductTransactionTypeOptions? transactionType)
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
