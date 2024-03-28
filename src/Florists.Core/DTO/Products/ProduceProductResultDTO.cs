using Florists.Core.Entities;

namespace Florists.Core.DTO.Products
{
  public record ProduceProductResultDTO(
    string Message,
    ProductTransaction ProductTransaction,
    List<InventoryTransaction> InventoryTransactions);
}
