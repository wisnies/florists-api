using Florists.Core.Entities;

namespace Florists.Core.DTO.ProductTransactions
{
  public record ProductTransactionsResultDTO(
    string Message,
    int Count,
    List<ProductTransaction> Transactions);
}
