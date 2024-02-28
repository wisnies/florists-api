using Florists.Core.Entities;

namespace Florists.Application.Interfaces.Persistence
{
  public interface IFlowerTransactionRepository
  {
    Task<bool> PurchaseAsync(List<FlowerTransaction> transactions);
    Task<bool> ProduceAsync(List<FlowerTransaction> transactions);
  }
}
