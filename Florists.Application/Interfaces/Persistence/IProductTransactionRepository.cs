using Florists.Core.Entities;
using Florists.Core.Enums;

namespace Florists.Application.Interfaces.Persistence
{
  public interface IProductTransactionRepository
  {
    Task<int> CountAsync(
      DateTime? dateFrom,
      DateTime? dateTo,
      ProductTransactionTypeOptions? transactionType);
    Task<List<ProductTransaction>?> GetManyAsync(
      int offset,
      int perPage,
      DateTime? dateFrom,
      DateTime? dateTo,
      ProductTransactionTypeOptions? transactionType,
      TransactionsOrderByOptions? orderBy,
      OrderOptions? order);
    Task<bool> SellAsync(List<ProductTransaction> transactions);
  }
}
