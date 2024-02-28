using Florists.Application.Interfaces.Persistence;
using Florists.Core.Entities;
using Florists.Infrastructure.DTO;
using Florists.Infrastructure.Interfaces;
using Florists.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Florists.Infrastructure.Persistence
{
  public class FlowerTransactionRepository : IFlowerTransactionRepository
  {
    private readonly MySqlSettings _settings;
    private readonly IDataAccess _dataAccess;

    public FlowerTransactionRepository(
      IOptions<MySqlSettings> mySqlOptions,
      IDataAccess dataAccess)
    {
      _settings = mySqlOptions.Value;
      _dataAccess = dataAccess;
    }

    public Task<bool> ProduceAsync(List<FlowerTransaction> transactions)
    {
      throw new NotImplementedException();
    }

    public async Task<bool> PurchaseAsync(List<FlowerTransaction> transactions)
    {
      try
      {
        var queries = new List<DependantQueryDTO>();
        foreach (var transaction in transactions)
        {
          var sqlMaster = $"INSERT INTO {_settings.FlowerTransactionsTable} " +
            $"(transaction_id, flower_id, user_id, purchase_order_number, quantity_before, quantity_after, transaction_value, transaction_type, created_at) VALUES " +
            $"(@TransactionId, @FlowerId, @UserId, @PurchaseOrderNumber, @QuantityBefore, @QuantityAfter, @TransactionValue, @TransactionType, @CreatedAt)";

          var parametersMaster = new
          {
            TransactionId = transaction.FlowerTransactionId,
            transaction.FlowerId,
            transaction.UserId,
            transaction.PurchaseOrderNumber,
            transaction.QuantityBefore,
            transaction.QuantityAfter,
            transaction.TransactionValue,
            transaction.TransactionType,
            transaction.CreatedAt
          };

          var sqlSlave = $"Update {_settings.FlowersTable} " +
            $"SET available_quantity = @AvailableQuantity, updated_at = @UpdatedAt " +
            $"WHERE flower_id = @FlowerId";

          var quantityToPurchase = transaction.QuantityAfter - transaction.QuantityBefore;

          var parametersSlave = new
          {
            transaction.FlowerId,
            AvailableQuantity = transaction.Flower!.AvailableQuantity + quantityToPurchase,
            transaction.Flower.UpdatedAt
          };

          var query = new DependantQueryDTO(
            sqlMaster,
            parametersMaster,
            sqlSlave,
            parametersSlave);

          queries.Add(query);
        }

        var rowsAffected = await _dataAccess.SaveDataTransaction(queries);
        return rowsAffected > 0;
      }
      catch
      {
        return false;
      }
    }
  }
}
