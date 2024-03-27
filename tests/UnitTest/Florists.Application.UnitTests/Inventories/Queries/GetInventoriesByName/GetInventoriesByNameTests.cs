using Florists.Application.Features.Inventories.Queries.GetInventoriesByName;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.UnitTests.Inventories.Queries.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Inventories;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Inventories.Queries.GetInventoriesByName
{
  public class GetInventoriesByNameTests
  {
    private GetInventoriesByNameQueryHandler _handler;
    private Mock<IInventoryRepository> _mockInventoryRepository;

    [SetUp]
    public void SetUp()
    {
      _mockInventoryRepository = new Mock<IInventoryRepository>();
      _handler = new(_mockInventoryRepository.Object);
    }

    [TestCase(1, 1)]
    [TestCase(2, 2)]
    [TestCase(5, 1)]
    [TestCase(3, 10)]
    [TestCase(123, 1)]
    public async Task HandleGetInventoriesByName_WhenValidQuery_ShouldReturnInventoriesResultDTOWithValidCollectionLength(int perPage, int page)
    {
      var query = GetInventoriesByNameUtils.CreateQuery(perPage, page);
      var offset = query.PerPage * (query.Page - 1);
      var dbInventories = GetInventoriesByNameUtils.CreateInventories(offset, perPage);


      _mockInventoryRepository
        .Setup(m => m.GetManyByNameAsync(
          offset,
          query.PerPage,
          query.InventoryName))
        .Returns(Task.FromResult((List<Inventory>?)dbInventories));

      _mockInventoryRepository
        .Setup(m => m.CountByNameAsync(query.InventoryName))
        .Returns(Task.FromResult(Constants.Inventories.InventoriesCount));

      var result = await _handler.Handle(
        query,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(query, offset);

        _mockInventoryRepository
        .Verify(m => m.GetManyByNameAsync(
          offset,
          query.PerPage,
          query.InventoryName),
          Times.Once());

        _mockInventoryRepository
        .Verify(m => m.CountByNameAsync(query.InventoryName),
          Times.Once());
      });
    }

    [Test]
    public async Task HandleGetInventoriesByName_WhenUnableToFetchWithValidQuery_ShouldReturnDatabaseFetchError()
    {
      var query = GetInventoriesByNameUtils.CreateQuery();

      var offset = query.PerPage * (query.Page - 1);

      _mockInventoryRepository
        .Setup(m => m.GetManyByNameAsync(
          offset,
          query.PerPage,
          query.InventoryName))
        .Returns(Task.FromResult((List<Inventory>?)null));

      var result = await _handler.Handle(
        query,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.FetchError));

        _mockInventoryRepository
        .Verify(m => m.GetManyByNameAsync(
          offset,
          query.PerPage,
          query.InventoryName),
          Times.Once());
      });
    }
  }
}
