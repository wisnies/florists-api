using Florists.Core.DTO.ProductTransactions;

namespace Florists.Core.Contracts.ProductTransactions
{
  public record ProductTransactionsResponse(
    string Message,
    int Count,
    List<ProductTransactionDTO> Transactions);
}
