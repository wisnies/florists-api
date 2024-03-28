using Florists.Application.Features.ProductTransactions.Queries.GetProductTransactions;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.ProductyTransactions.Queries.TestUtils
{
  public static class GetProductTransactionsQueryUtils
  {
    public static GetProductTransactionsQuery CreateQuery(int perPage = 10, int page = 1)
    {
      return new GetProductTransactionsQuery(
        DateTime.Parse(Constants.ProductTransactions.DateFrom),
        DateTime.Parse(Constants.ProductTransactions.DateTo),
        Constants.ProductTransactions.TransactionType,
        Constants.InventoryTransactions.Order,
        Constants.InventoryTransactions.OrderBy,
        page,
        perPage);
    }

    public static List<ProductTransaction> CreateTransactions(int offset, int perPage)
    {
      var total = perPage >= Constants.ProductTransactions.TransactionsCount ?
        Constants.ProductTransactions.TransactionsCount :
        perPage;
      total += offset;

      var transactions = new List<ProductTransaction>();

      for (int i = offset; i < total; i++)
      {
        var inventory = new ProductTransaction
        {
          ProductTransactionId = Guid.Parse(Constants.ProductTransactions.TransactionId),
          ProductId = Guid.Parse(Constants.ProductTransactions.ProductId),
          UserId = Guid.Parse(Constants.ProductTransactions.UserId),
          SaleOrderNumber = Constants.ProductTransactions.SaleOrdernumberFromIndex(i),
          ProductionOrderNumber = Constants.ProductTransactions.ProductionOrderNumber,
          QuantityBefore = Constants.ProductTransactions.QuantityBefore,
          QuantityAfter = Constants.ProductTransactions.QuantityAfter,
          TransactionValue = Constants.ProductTransactions.TransactionValue,
          TransactionType = Constants.ProductTransactions.TransactionType,
          CreatedAt = DateTime.Parse(Constants.ProductTransactions.CreatedAt),
          User = new FloristsUser
          {
            UserId = Guid.Parse(Constants.ProductTransactions.UserId),
            FirstName = Constants.ProductTransactions.FirstName,
            LastName = Constants.ProductTransactions.LastName,
          },
          Product = new Product
          {
            ProductId = Guid.Parse(Constants.ProductTransactions.ProductId),
            ProductName = Constants.ProductTransactions.ProductName,
            UnitPrice = Constants.ProductTransactions.UnitPrice,
            Category = Constants.ProductTransactions.Category,
          }

        };
        transactions.Add(inventory);
      }

      return transactions;
    }
  }
}
