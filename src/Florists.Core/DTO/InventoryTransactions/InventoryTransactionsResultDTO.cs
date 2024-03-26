using Florists.Core.Entities;

namespace Florists.Core.DTO.InventoryTransactions
{
  public record InventoryTransactionsResultDTO(
    string Message,
    int Count,
    List<InventoryTransaction> Transactions);
}

