using Florists.Core.Entities;

namespace Florists.Application.Interfaces.Persistence
{
  public interface IProductTransactionRepository
  {
    Task<bool> SellAsync(List<ProductTransaction> transactions);
  }
}
