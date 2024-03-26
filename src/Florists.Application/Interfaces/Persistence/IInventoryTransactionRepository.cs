using Florists.Core.Entities;
using Florists.Core.Enums;

namespace Florists.Application.Interfaces.Persistence
{
  public interface IInventoryTransactionRepository
  {
    Task<int> CountAsync(
      DateTime? dateFrom,
      DateTime? dateTo,
      InventoryTransactionTypeOptions? transactionType);
    Task<List<InventoryTransaction>?> GetManyAsync(
      int offset,
      int perPage,
      DateTime? dateFrom,
      DateTime? dateTo,
      InventoryTransactionTypeOptions? transactionType,
      TransactionsOrderByOptions? orderBy,
      OrderOptions? orderOptions);
    Task<bool> PurchaseAsync(List<InventoryTransaction> transactions);
  }
}
