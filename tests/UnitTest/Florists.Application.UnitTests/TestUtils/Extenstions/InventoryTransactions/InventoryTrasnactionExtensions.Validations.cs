using Florists.Application.Features.InventoryTransactions.Queries.GetInventoryTransactions;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.InventoryTransactions;

namespace Florists.Application.UnitTests.TestUtils.Extenstions.InventoryTransactions
{
  public static partial class InventoryTrasnactionExtensions
  {
    public static void ValidateCreatedFrom(this InventoryTransactionsResultDTO result, GetInventoryTransactionsQuery query, int offset)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.FetchSuccess));
        Assert.That(result.Count, Is.EqualTo(Constants.Constants.InventoryTransactions.TransactionsCount));
        Assert.That(result.Transactions, Has.Count.LessThanOrEqualTo(query.PerPage));

        if (query.PerPage >= Constants.Constants.InventoryTransactions.TransactionsCount)
        {
          Assert.That(result.Transactions, Has.Count.AtLeast(Constants.Constants.InventoryTransactions.TransactionsCount));
        }

        for (int i = 0; i < result.Transactions.Count; i++)
        {
          var n = offset + i;
          Assert.That(
            result.Transactions[i].PurchaseOrderNumber,
            Is.EqualTo(Constants.Constants.InventoryTransactions.PurchaseOrdernumberFromIndex(n)));
        }
      });
    }
  }
}
