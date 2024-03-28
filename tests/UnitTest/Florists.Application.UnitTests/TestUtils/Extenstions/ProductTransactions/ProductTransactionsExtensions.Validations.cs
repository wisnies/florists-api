using Florists.Application.Features.ProductTransactions.Queries.GetProductTransactions;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.ProductTransactions;

namespace Florists.Application.UnitTests.TestUtils.Extenstions.ProductTransactions
{
  public static partial class ProductTrasnactionExtensions
  {
    public static void ValidateCreatedFrom(this ProductTransactionsResultDTO result, GetProductTransactionsQuery query, int offset)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.FetchSuccess));
        Assert.That(result.Count, Is.EqualTo(Constants.Constants.ProductTransactions.TransactionsCount));
        Assert.That(result.Transactions, Has.Count.LessThanOrEqualTo(query.PerPage));

        if (query.PerPage >= Constants.Constants.ProductTransactions.TransactionsCount)
        {
          Assert.That(result.Transactions, Has.Count.AtLeast(Constants.Constants.ProductTransactions.TransactionsCount));
        }

        for (int i = 0; i < result.Transactions.Count; i++)
        {
          var n = offset + i;
          Assert.That(
            result.Transactions[i].SaleOrderNumber,
            Is.EqualTo(Constants.Constants.ProductTransactions.SaleOrdernumberFromIndex(n)));
        }
      });
    }
  }
}
