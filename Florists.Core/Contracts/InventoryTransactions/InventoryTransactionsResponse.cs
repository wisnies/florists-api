using Florists.Core.DTO.InventoryTransactions;

namespace Florists.Core.Contracts.InventoryTransactions
{
  public record InventoryTransactionsResponse(
    string Message,
    int Count,
    List<InventoryTransactionDTO> Transactions);
}
