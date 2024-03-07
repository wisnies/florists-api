using Florists.Core.Entities;

namespace Florists.Application.Interfaces.Persistence
{
  public interface IInventoryTransactionRepository
  {
    Task<bool> PurchaseAsync(List<InventoryTransaction> transactions);
    Task<bool> ProduceAsync(List<InventoryTransaction> transactions);
  }
}
