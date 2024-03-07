using Florists.Application.Interfaces.Persistence;
using Florists.Core.Entities;
using Florists.Infrastructure.DTO.Common;
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

    public Task<bool> ProduceAsync(List<InventoryTransaction> transactions)
    {
      throw new NotImplementedException();
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
  }
}
