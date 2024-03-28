using Florists.Application.Features.ProductTransactions.Queries.GetProductTransactions;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.UnitTests.ProductyTransactions.Queries.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.InventoryTransactions;
using Florists.Application.UnitTests.TestUtils.Extenstions.ProductTransactions;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.ProductTransactions.Queries.GetProductTransactions
{
  public class GetProductTransactionsQueryTests
  {
    private GetProductTransactionsQueryHandler _handler;
    private Mock<IProductTransactionRepository> _mockProductTransactionRepository;

    [SetUp]
    public void SetUp()
    {
      _mockProductTransactionRepository = new Mock<IProductTransactionRepository>();
      _handler = new(_mockProductTransactionRepository.Object);
    }

    [Test]
    public async Task HandleGetProductTransactions_WhenUnableToFetchWithValidQuery_ShouldReturnDatabaseFetchError()
    {
      var query = GetProductTransactionsQueryUtils.CreateQuery();

      var offset = query.PerPage * (query.Page - 1);

      _mockProductTransactionRepository
        .Setup(m => m.GetManyAsync(
          offset,
          query.PerPage,
          query.DateFrom,
          query.DateTo,
          query.TransactionType,
          query.OrderBy,
          query.Order))
        .Returns(Task.FromResult((List<ProductTransaction>?)null));

      var result = await _handler.Handle(
        query,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.FetchError));

        _mockProductTransactionRepository
        .Verify(m => m.GetManyAsync(
          offset,
          query.PerPage,
          query.DateFrom,
          query.DateTo,
          query.TransactionType,
          query.OrderBy,
          query.Order), Times.Once());
      });
    }

    [TestCase(1, 1)]
    [TestCase(2, 2)]
    [TestCase(5, 1)]
    [TestCase(3, 10)]
    [TestCase(16, 1)]
    public async Task HandleGetProductTransactions_WhenValidQuery_ShouldReturnProductTransactionsResultDTOWithValidCollectionLength(int perPage, int page)
    {
      var query = GetProductTransactionsQueryUtils.CreateQuery(perPage, page);

      var offset = query.PerPage * (query.Page - 1);

      var dbTransactions = GetProductTransactionsQueryUtils.CreateTransactions(offset, perPage);

      _mockProductTransactionRepository
        .Setup(m => m.GetManyAsync(
          offset,
          query.PerPage,
          query.DateFrom,
          query.DateTo,
          query.TransactionType,
          query.OrderBy,
          query.Order))
        .Returns(Task.FromResult((List<ProductTransaction>?)dbTransactions));

      _mockProductTransactionRepository
        .Setup(m => m.CountAsync(
          query.DateFrom,
          query.DateTo,
          query.TransactionType))
        .Returns(Task.FromResult(Constants.InventoryTransactions.TransactionsCount));

      var result = await _handler.Handle(
        query,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(query, offset);

        _mockProductTransactionRepository
        .Verify(m => m.GetManyAsync(
          offset,
          query.PerPage,
          query.DateFrom,
          query.DateTo,
          query.TransactionType,
          query.OrderBy,
          query.Order), Times.Once());

        _mockProductTransactionRepository
        .Verify(m => m.CountAsync(
          query.DateFrom,
          query.DateTo,
          query.TransactionType), Times.Once());
      });
    }
  }
}
