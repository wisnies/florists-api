using Florists.Application.Features.InventoryTransactions.Queries.GetInventoryTransactions;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.InventoryTransactions.Queries.TestUtils
{
  public static class GetInventoryTransactionsQueryUtils
  {
    public static GetInventoryTransactionsQuery CreateQuery(int perPage = 10, int page = 1)
    {
      return new GetInventoryTransactionsQuery(
        DateTime.Parse(Constants.InventoryTransactions.DateFrom),
        DateTime.Parse(Constants.InventoryTransactions.DateTo),
        Constants.InventoryTransactions.TransactionType,
        Constants.InventoryTransactions.Order,
        Constants.InventoryTransactions.OrderBy,
        page,
        perPage);
    }

    public static List<InventoryTransaction> CreateTransactions(int offset, int perPage)
    {
      var total = perPage >= Constants.InventoryTransactions.TransactionsCount ?
        Constants.InventoryTransactions.TransactionsCount :
        perPage;
      total += offset;

      var transactions = new List<InventoryTransaction>();

      for (int i = offset; i < total; i++)
      {
        var inventory = new InventoryTransaction
        {
          InventoryTransactionId = Guid.Parse(Constants.InventoryTransactions.TransactionId),
          InventoryId = Guid.Parse(Constants.InventoryTransactions.InventoryId),
          UserId = Guid.Parse(Constants.InventoryTransactions.UserId),
          PurchaseOrderNumber = Constants.InventoryTransactions.PurchaseOrdernumberFromIndex(i),
          ProductionOrderNumber = Constants.InventoryTransactions.ProductionOrderNumber,
          QuantityBefore = Constants.InventoryTransactions.QuantityBefore,
          QuantityAfter = Constants.InventoryTransactions.QuantityAfter,
          TransactionValue = Constants.InventoryTransactions.TransactionValue,
          TransactionType = Constants.InventoryTransactions.TransactionType,
          CreatedAt = DateTime.Parse(Constants.InventoryTransactions.CreatedAt),
          User = new FloristsUser
          {
            UserId = Guid.Parse(Constants.InventoryTransactions.UserId),
            FirstName = Constants.InventoryTransactions.FirstName,
            LastName = Constants.InventoryTransactions.LastName,
          },
          Inventory = new Inventory
          {
            InventoryId = Guid.Parse(Constants.InventoryTransactions.InventoryId),
            InventoryName = Constants.InventoryTransactions.InventoryName,
            UnitPrice = Constants.InventoryTransactions.UnitPrice,
            Category = Constants.InventoryTransactions.Category,
          }

        };
        transactions.Add(inventory);
      }

      return transactions;
    }
  }
}
