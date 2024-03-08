using Florists.Application.Interfaces.Persistence;
using Florists.Core.Entities;
using Florists.Infrastructure.DTO.Common;
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
  }
}
