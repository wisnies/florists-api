using Florists.Application.Features.Inventories.Commands.EditInventory;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Application.UnitTests.Inventories.Commands.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Inventories;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Inventories.Commands.EditInventory
{
    public class EditInventoryCommandTests
  {
    private EditInventoryCommandHandler _handler;
    private Mock<IInventoryRepository> _mockInventoryRepository;
    private Mock<IDateTimeService> _mockDateTimeService;

    [SetUp]
    public void SetUp()
    {
      _mockDateTimeService = new Mock<IDateTimeService>();
      _mockInventoryRepository = new Mock<IInventoryRepository>();

      _handler = new(
        _mockInventoryRepository.Object,
        _mockDateTimeService.Object);
    }

    [Test]
    public async Task HandleEditInventoryCommand_WhenInventoryNotFound_ShouldReturnInventoryNotFoundError()
    {
      var command = EditInventoryCommandUtils.CreateCommand();

      _mockInventoryRepository
        .Setup(m => m.GetOneByIdAsync(command.InventoryId))
        .Returns(Task.FromResult((Inventory?)null));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Inventories.NotFound));

        _mockInventoryRepository
        .Verify(m => m.GetOneByIdAsync(command.InventoryId), Times.Once());
      });
    }

    [Test]
    public async Task HandleEditInventoryCommand_WhenInventoryWithEditedNameAlreadyExists_ShouldReturnInventoryAlreadyExists()
    {
      var command = EditInventoryCommandUtils.CreateCommand();
      var dbInventory = EditInventoryCommandUtils.CreateInventory();
      var dbSameNameInventory = EditInventoryCommandUtils.CreateInventory(true);

      _mockInventoryRepository
        .Setup(m => m.GetOneByIdAsync(command.InventoryId))
        .Returns(Task.FromResult((Inventory?)dbInventory));

      _mockInventoryRepository
        .Setup(m => m.GetOneByNameAsync(command.InventoryName))
        .Returns(Task.FromResult((Inventory?)dbSameNameInventory));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Inventories.AlreadyExists));

        _mockInventoryRepository
        .Verify(m => m.GetOneByIdAsync(command.InventoryId), Times.Once());

        _mockInventoryRepository
        .Verify(m => m.GetOneByNameAsync(command.InventoryName), Times.Once());
      });
    }

    [Test]
    public async Task HandleEditInventoryCommand_WhenUnableToEditWithValidCommand_ShouldReturnDatabaseSaveError()
    {
      var command = EditInventoryCommandUtils.CreateCommand();
      var dbInventory = EditInventoryCommandUtils.CreateInventory();

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Inventories.EditedUtcNow));

      _mockInventoryRepository
        .Setup(m => m.GetOneByIdAsync(command.InventoryId))
        .Returns(Task.FromResult((Inventory?)dbInventory));

      _mockInventoryRepository
        .Setup(m => m.GetOneByNameAsync(command.InventoryName))
        .Returns(Task.FromResult((Inventory?)null));

      _mockInventoryRepository
        .Setup(m => m.UpdateAsync(dbInventory))
        .Returns(Task.FromResult(false));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.SaveError));

        _mockDateTimeService
        .Verify(m => m.UtcNow, Times.Once());

        _mockInventoryRepository
        .Verify(m => m.GetOneByIdAsync(command.InventoryId), Times.Once());

        _mockInventoryRepository
        .Verify(m => m.GetOneByNameAsync(command.InventoryName), Times.Once());

        _mockInventoryRepository
        .Verify(m => m.UpdateAsync(dbInventory), Times.Once());
      });
    }

    [Test]
    public async Task HandleEditInventoryCommand_ValidCommand_ShouldReturnInventoryResultDTO()
    {
      var command = EditInventoryCommandUtils.CreateCommand();
      var dbInventory = EditInventoryCommandUtils.CreateInventory();

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Inventories.EditedUtcNow));

      _mockInventoryRepository
        .Setup(m => m.GetOneByIdAsync(command.InventoryId))
        .Returns(Task.FromResult((Inventory?)dbInventory));

      _mockInventoryRepository
        .Setup(m => m.GetOneByNameAsync(command.InventoryName))
        .Returns(Task.FromResult((Inventory?)null));

      _mockInventoryRepository
        .Setup(m => m.UpdateAsync(dbInventory))
        .Returns(Task.FromResult(true));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(command);

        _mockDateTimeService
        .Verify(m => m.UtcNow, Times.Once());

        _mockInventoryRepository
        .Verify(m => m.GetOneByIdAsync(command.InventoryId), Times.Once());

        _mockInventoryRepository
        .Verify(m => m.GetOneByNameAsync(command.InventoryName), Times.Once());

        _mockInventoryRepository
        .Verify(m => m.UpdateAsync(dbInventory), Times.Once());
      });
    }
  }
}
