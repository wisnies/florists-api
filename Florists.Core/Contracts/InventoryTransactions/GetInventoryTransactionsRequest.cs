using Florists.Core.Enums;

namespace Florists.Core.Contracts.InventoryTransactions
{
  public record GetInventoryTransactionsRequest(
    DateTime? DateFrom,
    DateTime? DateTo,
    InventoryTransactionTypeOptions? TransactionType,
    TransactionsOrderByOptions? OrderBy,
    OrderOptions? Order,
    int Page,
    int PerPage);
}
