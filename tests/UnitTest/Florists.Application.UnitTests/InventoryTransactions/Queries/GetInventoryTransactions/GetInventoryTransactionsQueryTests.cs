using Florists.Application.Features.InventoryTransactions.Queries.GetInventoryTransactions;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.UnitTests.InventoryTransactions.Queries.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.InventoryTransactions;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.InventoryTransactions.Queries.GetInventoryTransactions
{
  public class GetInventoryTransactionsQueryTests
  {
    private GetInventoryTransactionsQueryHandler _handler;
    private Mock<IInventoryTransactionRepository> _mockInventoryTransactionRepository;

    [SetUp]
    public void SetUp()
    {
      _mockInventoryTransactionRepository = new Mock<IInventoryTransactionRepository>();
      _handler = new(_mockInventoryTransactionRepository.Object);
    }

    [Test]
    public async Task HandleGetInventoryTransactions_WhenUnableToFetchWithValidQuery_ShouldReturnDatabaseFetchError()
    {
      var query = GetInventoryTransactionsQueryUtils.CreateQuery();

      var offset = query.PerPage * (query.Page - 1);

      _mockInventoryTransactionRepository
        .Setup(m => m.GetManyAsync(
          offset,
          query.PerPage,
          query.DateFrom,
          query.DateTo,
          query.TransactionType,
          query.OrderBy,
          query.Order))
        .Returns(Task.FromResult((List<InventoryTransaction>?)null));

      var result = await _handler.Handle(
        query,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.FetchError));

        _mockInventoryTransactionRepository
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
    public async Task HandleGetInventoryTransactions_WhenValidQuery_ShouldReturnInventoryTransactionsResultDTOWithValidCollectionLength(int perPage, int page)
    {
      var query = GetInventoryTransactionsQueryUtils.CreateQuery(perPage, page);

      var offset = query.PerPage * (query.Page - 1);

      var dbTransactions = GetInventoryTransactionsQueryUtils.CreateTransactions(offset, perPage);

      _mockInventoryTransactionRepository
        .Setup(m => m.GetManyAsync(
          offset,
          query.PerPage,
          query.DateFrom,
          query.DateTo,
          query.TransactionType,
          query.OrderBy,
          query.Order))
        .Returns(Task.FromResult((List<InventoryTransaction>?)dbTransactions));

      _mockInventoryTransactionRepository
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

        _mockInventoryTransactionRepository
        .Verify(m => m.GetManyAsync(
          offset,
          query.PerPage,
          query.DateFrom,
          query.DateTo,
          query.TransactionType,
          query.OrderBy,
          query.Order), Times.Once());

        _mockInventoryTransactionRepository
        .Verify(m => m.CountAsync(
          query.DateFrom,
          query.DateTo,
          query.TransactionType), Times.Once());
      });
    }
  }
}
