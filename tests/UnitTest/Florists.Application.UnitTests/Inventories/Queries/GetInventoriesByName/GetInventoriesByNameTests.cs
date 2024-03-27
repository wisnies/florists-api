using Florists.Application.Features.Inventories.Queries.GetInventoriesByName;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.UnitTests.Inventories.Queries.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Inventories.Extenstions;
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

    [TestCase(1)]
    [TestCase(12)]
    [TestCase(14)]
    [TestCase(5)]
    [TestCase(0)]
    public async Task HandleGetInventoriesByName_WithPerPageParameter_ShouldReturnInventoriesResultDTOWithValidLength(int perPage)
    {
      var getInventoriesByNameQuery = GetInventoriesByNameUtils.CreateQuery(perPage: perPage);
      var dbInventories = GetInventoriesByNameUtils.CreateInventories(perPage);

      var offset = getInventoriesByNameQuery.PerPage * (getInventoriesByNameQuery.Page - 1);

      _mockInventoryRepository
        .Setup(m => m.GetManyByNameAsync(
          offset,
          getInventoriesByNameQuery.PerPage,
          getInventoriesByNameQuery.InventoryName))
        .Returns(Task.FromResult((List<Inventory>?)dbInventories));

      _mockInventoryRepository
        .Setup(m => m.CountByNameAsync(getInventoriesByNameQuery.InventoryName))
        .Returns(Task.FromResult(Constants.Inventories.InventoriesCount));

      var result = await _handler.Handle(
        getInventoriesByNameQuery,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(getInventoriesByNameQuery);

        _mockInventoryRepository
        .Verify(m => m.GetManyByNameAsync(
          offset,
          getInventoriesByNameQuery.PerPage,
          getInventoriesByNameQuery.InventoryName),
          Times.Once());

        _mockInventoryRepository
        .Verify(m => m.CountByNameAsync(getInventoriesByNameQuery.InventoryName),
          Times.Once());
      });
    }

    [Test]
    public async Task HandleGetInventoriesByName_WhenUnableToFetch_ShouldReturnDatabaseFetchError()
    {
      var getInventoriesByNameQuery = GetInventoriesByNameUtils.CreateQuery();

      var offset = getInventoriesByNameQuery.PerPage * (getInventoriesByNameQuery.Page - 1);

      _mockInventoryRepository
        .Setup(m => m.GetManyByNameAsync(
          offset,
          getInventoriesByNameQuery.PerPage,
          getInventoriesByNameQuery.InventoryName))
        .Returns(Task.FromResult((List<Inventory>?)null));

      var result = await _handler.Handle(
        getInventoriesByNameQuery,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.FetchError));

        _mockInventoryRepository
        .Verify(m => m.GetManyByNameAsync(
          offset,
          getInventoriesByNameQuery.PerPage,
          getInventoriesByNameQuery.InventoryName),
          Times.Once());
      });
    }
  }
}
