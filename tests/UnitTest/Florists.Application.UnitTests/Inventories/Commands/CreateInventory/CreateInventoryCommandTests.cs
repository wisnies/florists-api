using Florists.Application.Features.Inventories.Commands.CreateInventory;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Application.UnitTests.Inventories.Commands.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Inventories;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Inventories.Commands.CreateInventory
{
  public class CreateInventoryCommandTests
  {
    private CreateInventoryCommandHandler _handler;
    private Mock<IInventoryRepository> _mockInventoryRepository;
    private Mock<IDateTimeService> _mockDateTimeService;

    [SetUp]
    public void SetUp()
    {
      _mockInventoryRepository = new Mock<IInventoryRepository>();
      _mockDateTimeService = new Mock<IDateTimeService>();

      _handler = new(
        _mockInventoryRepository.Object,
        _mockDateTimeService.Object);
    }

    [Test]
    public async Task HandleCreateInventoryCommand_WhenInventoryWithNameAlreadyExists_ShouldReturnInventoryAlreadyExistsError()
    {
      var createInventoryComand = CreateInventoryCommandUtils.CreateCommand();
      var dbInventory = CreateInventoryCommandUtils.CreateInventory();

      _mockInventoryRepository
        .Setup(m => m.GetOneByNameAsync(createInventoryComand.InventoryName))
        .Returns(Task.FromResult((Inventory?)dbInventory));

      var result = await _handler.Handle(
        createInventoryComand,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Inventories.AlreadyExists));

        _mockInventoryRepository
        .Verify(
          m => m.GetOneByNameAsync(createInventoryComand.InventoryName),
          Times.Once());
      });
    }

    [Test]
    public async Task HandleCreateInventoryCommand_WhenUnableCreateInventoryWithValidCommand_ShouldReturnDatabaseSaveError()
    {
      var createInventoryComand = CreateInventoryCommandUtils.CreateCommand();

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Inventories.UtcNow));

      _mockInventoryRepository
        .Setup(m => m.GetOneByNameAsync(createInventoryComand.InventoryName))
        .Returns(Task.FromResult((Inventory?)null));

      _mockInventoryRepository
        .Setup(m => m.CreateAsync(It.IsAny<Inventory>()))
        .Returns(Task.FromResult(false));

      var result = await _handler.Handle(
        createInventoryComand,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.SaveError));

        _mockDateTimeService
        .Verify(m => m.UtcNow, Times.Once());

        _mockInventoryRepository
        .Verify(
          m => m.GetOneByNameAsync(createInventoryComand.InventoryName),
          Times.Once());

        _mockInventoryRepository
          .Verify(m => m.CreateAsync(
            It.IsAny<Inventory>()),
            Times.Once());
      });
    }

    [Test]
    public async Task HandleCreateInventoryCommand_WhenCommandIsValid_ShouldReturnInventoryResultDTO()
    {
      var createInventoryComand = CreateInventoryCommandUtils.CreateCommand();

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Inventories.UtcNow));

      _mockInventoryRepository
        .Setup(m => m.GetOneByNameAsync(createInventoryComand.InventoryName))
        .Returns(Task.FromResult((Inventory?)null));

      _mockInventoryRepository
        .Setup(m => m.CreateAsync(It.IsAny<Inventory>()))
        .Returns(Task.FromResult(true));

      var result = await _handler.Handle(
        createInventoryComand,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(createInventoryComand);

        _mockDateTimeService
        .Verify(m => m.UtcNow, Times.Once());

        _mockInventoryRepository
        .Verify(
          m => m.GetOneByNameAsync(createInventoryComand.InventoryName),
          Times.Once());

        _mockInventoryRepository
          .Verify(m => m.CreateAsync(It.IsAny<Inventory>()), Times.Once());
      });
    }
  }
}
