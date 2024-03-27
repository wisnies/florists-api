using Florists.Application.Features.Inventories.Queries.GetInventoryById;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.UnitTests.Inventories.Queries.TestUtils;
using Florists.Application.UnitTests.TestUtils.Inventories.Extenstions;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Inventories.Queries.GetInventoryById
{
  public class GetInventoryByIdQueryTests
  {
    private GetInventoryByIdQueryHandler _handler;
    private Mock<IInventoryRepository> _mockInventoryRepository;

    [SetUp]
    public void SetUp()
    {
      _mockInventoryRepository = new Mock<IInventoryRepository>();
      _handler = new(_mockInventoryRepository.Object);
    }

    [Test]
    public async Task HandleGetInventoryByIdQuery_WhenIdIsValid_ShouldReturnInventoryResultDTO()
    {
      var getInventoryByIdQuery = GetInventoryByIdQueryUtils.CreateQuery();
      var dbInventory = GetInventoryByIdQueryUtils.CreateInventory();

      _mockInventoryRepository
        .Setup(m => m.GetOneByIdAsync(getInventoryByIdQuery.InventoryId))
        .Returns(Task.FromResult((Inventory?)dbInventory));

      var result = await _handler.Handle(getInventoryByIdQuery, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(getInventoryByIdQuery);

        _mockInventoryRepository.Verify(
          m => m.GetOneByIdAsync(getInventoryByIdQuery.InventoryId),
          Times.Once());
      });
    }

    [Test]
    public async Task HandleGetInventoryByIdQuery_WhenIdIsInvalid_ShouldReturnInventoryNotFoundError()
    {
      var getInventoryByIdQuery = GetInventoryByIdQueryUtils.CreateQuery();

      _mockInventoryRepository
        .Setup(m => m.GetOneByIdAsync(getInventoryByIdQuery.InventoryId))
        .Returns(Task.FromResult((Inventory?)null));

      var result = await _handler.Handle(getInventoryByIdQuery, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(
          result.FirstError,
          Is.EqualTo(CustomErrors.Inventories.NotFound));


        _mockInventoryRepository.Verify(
          m => m.GetOneByIdAsync(getInventoryByIdQuery.InventoryId),
          Times.Once());
      });
    }
  }
}
