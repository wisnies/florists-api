using Florists.Core.Enums;

namespace Florists.Core.Contracts.ProductTransactions
{
  public record GetProductTransactionsRequest(
    DateTime? DateFrom,
    DateTime? DateTo,
    ProductTransactionTypeOptions? TransactionType,
    TransactionsOrderByOptions? OrderBy,
    OrderOptions? Order,
    int Page,
    int PerPage);
}
