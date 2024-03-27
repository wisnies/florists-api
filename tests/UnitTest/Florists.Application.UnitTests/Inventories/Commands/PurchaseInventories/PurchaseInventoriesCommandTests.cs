using Florists.Application.Features.Inventories.Commands.PurchaseInventories;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
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
    public async Task HandlePurchaseInventoriesCommand_WhenUserWithEmailIsNotFound_ShouldReturnUserNotFoundError()
    {

    }

    [Test]
    public async Task HandlePurchaseInventoriesCommand_WhenOnOfInventoriesIsNotFound_ShouldReturnInventoryNotFoundError()
    {

    }

    [Test]
    public async Task HandlePurchaseInventoriesCommand_WhenUnableToPurchaseInventoriesWithValidCommand_ShouldReturnDatabaseSaveError()
    {

    }
  }
}
