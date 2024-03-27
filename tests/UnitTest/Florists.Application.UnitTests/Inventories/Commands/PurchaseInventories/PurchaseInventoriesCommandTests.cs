using Florists.Application.Features.Inventories.Commands.PurchaseInventories;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Application.UnitTests.Inventories.Commands.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Inventories;
using Florists.Application.UnitTests.TestUtils.Extenstions.InventoryTransactions;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Inventories.Commands.PurchaseInventories
{
  public class PurchaseInventoriesCommandTests
  {
    private PurchaseInventoriesCommandHandler _handler;
    private Mock<IInventoryRepository> _mockInventoryRepository;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IDateTimeService> _mockDateTimeService;
    private Mock<IInventoryTransactionRepository> _mockInventoryTransactionRepository;

    [SetUp]
    public void SetUp()
    {
      _mockInventoryRepository = new Mock<IInventoryRepository>();
      _mockUserRepository = new Mock<IUserRepository>();
      _mockDateTimeService = new Mock<IDateTimeService>();
      _mockInventoryTransactionRepository = new Mock<IInventoryTransactionRepository>();

      _handler = new(
        _mockInventoryRepository.Object,
        _mockDateTimeService.Object,
        _mockUserRepository.Object,
        _mockInventoryTransactionRepository.Object);
    }

    [Test]
    public async Task HandlePurchaseInventoriesCommand_WhenUserNotFoundByEmail_ShouldReturnUserNotFoundError()
    {
      var command = PurchaseInventoriesCommandUtils.CreateCommand();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)null));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Users.NotFound));

        _mockUserRepository
        .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());
      });
    }

    [Test]
    public async Task HandlePurchaseInventoriesCommand_WhenOnOfInventoriesIsNotFound_ShouldReturnInventoryNotFoundError()
    {
      var command = PurchaseInventoriesCommandUtils.CreateCommand();
      var dbUser = PurchaseInventoriesCommandUtils.CreateUser();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockInventoryRepository
        .Setup(m => m.GetOneByIdAsync(It.IsAny<Guid>()))
        .Returns(Task.FromResult((Inventory?)null));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Inventories.NotFound));

        _mockUserRepository
          .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());

        _mockInventoryRepository
        .Verify(m => m.GetOneByIdAsync(It.IsAny<Guid>()), Times.AtLeastOnce());
      });
    }

    [Test]
    public async Task HandlePurchaseInventoriesCommand_WhenUnableToPurchaseInventoriesWithValidCommand_ShouldReturnDatabaseSaveError()
    {
      var command = PurchaseInventoriesCommandUtils.CreateCommand();
      var dbUser = PurchaseInventoriesCommandUtils.CreateUser();
      var dbInventory = PurchaseInventoriesCommandUtils.CreateInventory();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockInventoryRepository
        .Setup(m => m.GetOneByIdAsync(It.IsAny<Guid>()))
        .Returns(Task.FromResult((Inventory?)dbInventory));

      _mockInventoryTransactionRepository
        .Setup(m => m.PurchaseAsync(It.IsAny<List<InventoryTransaction>>()))
        .Returns(Task.FromResult((false)));

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Inventories.UtcNow));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.SaveError));

        _mockUserRepository
          .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());

        _mockInventoryRepository
        .Verify(m => m.GetOneByIdAsync(
          It.IsAny<Guid>()),
          Times.Exactly(command.InventoriesToPurchase.Count));

        _mockInventoryTransactionRepository
        .Verify(m => m.PurchaseAsync(It.IsAny<List<InventoryTransaction>>()), Times.Once());
      });
    }

    [TestCase(1)]
    [TestCase(7)]
    [TestCase(9)]
    [TestCase(27)]
    [TestCase(14)]
    public async Task HandlePurchaseInventoriesCommand_WhenValidCommand_ShouldReturnInventoryTransactionsResultDTOWithValidCollectionLength(int count)
    {
      var command = PurchaseInventoriesCommandUtils.CreateCommand(count);
      var dbUser = PurchaseInventoriesCommandUtils.CreateUser();
      var dbInventory = PurchaseInventoriesCommandUtils.CreateInventory();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      foreach (var inventoryToPurchase in command.InventoriesToPurchase)
      {
        _mockInventoryRepository
          .Setup(m => m.GetOneByIdAsync(inventoryToPurchase.InventoryId))
          .Returns(Task.FromResult((Inventory?)PurchaseInventoriesCommandUtils.CreateInventory()));
      }

      _mockInventoryTransactionRepository
        .Setup(m => m.PurchaseAsync(It.IsAny<List<InventoryTransaction>>()))
        .Returns(Task.FromResult((true)));

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Inventories.EditedUtcNow));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(command);

        _mockUserRepository
          .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());

        _mockInventoryRepository
        .Verify(m => m.GetOneByIdAsync(
          It.IsAny<Guid>()),
          Times.Exactly(command.InventoriesToPurchase.Count));

        _mockInventoryTransactionRepository
        .Verify(m => m.PurchaseAsync(It.IsAny<List<InventoryTransaction>>()), Times.Once());
      });
    }
  }
}
