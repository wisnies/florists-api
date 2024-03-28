using Florists.Application.Features.Products.Commands.ProduceProduct;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Application.UnitTests.Products.Commands.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Products;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Products.Commands.ProduceProduct
{
  public class ProduceProductsCommandTests
  {
    private ProduceProductCommandHandler _handler;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IDateTimeService> _mockDateTimeService;
    private Mock<IProductRepository> _mockProductRepository;

    [SetUp]
    public void SetUp()
    {
      _mockDateTimeService = new Mock<IDateTimeService>();
      _mockUserRepository = new Mock<IUserRepository>();
      _mockProductRepository = new Mock<IProductRepository>();

      _handler = new(
        _mockProductRepository.Object,
        _mockUserRepository.Object,
        _mockDateTimeService.Object);
    }

    [Test]
    public async Task HandleProduceProductCommand_WhenUserNotFoundByEmail_ShouldReturnUserNotFoundError()
    {
      var command = ProducteProductCommandUtils.CreateCommand();

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
    public async Task HandleProduceProductCommand_WhenProductNotFoundById_ShouldReturnProductNotFoundError()
    {
      var command = ProducteProductCommandUtils.CreateCommand();
      var dbUser = ProducteProductCommandUtils.CreateUser();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(command.ProductId, true))
        .Returns(Task.FromResult((Product?)null));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Products.NotFound));

        _mockUserRepository
        .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());

        _mockProductRepository
        .Verify(m => m.GetOneByIdAsync(command.ProductId, true), Times.Once());
      });
    }

    [Test]
    public async Task HandleProduceProductCommand_WhenRequiredInventoryHasInsufficientQuantity_ShouldReturnInventoryInsufficientQuantityError()
    {
      var command = ProducteProductCommandUtils.CreateCommand();
      var dbUser = ProducteProductCommandUtils.CreateUser();
      var dbProduct = ProducteProductCommandUtils.CreateProduct(withInsufficientInventories: true);
      var productInventory = dbProduct.ProductInventories!.FirstOrDefault(x => x.Inventory!.AvailableQuantity == -1);

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(command.ProductId, true))
        .Returns(Task.FromResult((Product?)dbProduct));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Inventories.InsufficientQuantity(productInventory!.Inventory!.InventoryName)));

        _mockUserRepository
        .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());

        _mockProductRepository
        .Verify(m => m.GetOneByIdAsync(command.ProductId, true), Times.Once());
      });
    }

    [Test]
    public async Task HandleProduceProductCommand_WhenUnableToProduceWithValidCommand_ShouldReturnDatabaseSaveError()
    {
      var command = ProducteProductCommandUtils.CreateCommand();
      var dbUser = ProducteProductCommandUtils.CreateUser();
      var dbProduct = ProducteProductCommandUtils.CreateProduct();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(command.ProductId, true))
        .Returns(Task.FromResult((Product?)dbProduct));

      _mockProductRepository
        .Setup(m => m.ProduceAsync(
          It.IsAny<ProductTransaction>(),
          It.IsAny<List<InventoryTransaction>>()))
        .Returns(Task.FromResult(false));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.SaveError));

        _mockUserRepository
        .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());

        _mockProductRepository
        .Verify(m => m.GetOneByIdAsync(command.ProductId, true), Times.Once());

        _mockProductRepository
          .Verify(m => m.ProduceAsync(
            It.IsAny<ProductTransaction>(),
            It.IsAny<List<InventoryTransaction>>()), Times.Once());
      });
    }

    [TestCase(1, 1)]
    [TestCase(2, 3)]
    [TestCase(9, 3)]
    [TestCase(5, 8)]
    public async Task HandleProduceProductCommand_WhenValidCommand_ShouldReturnProduceProductResultDTO(int productCount, int inventoriesCount)
    {
      var command = ProducteProductCommandUtils.CreateCommand(productCount);
      var dbUser = ProducteProductCommandUtils.CreateUser();
      var dbProduct = ProducteProductCommandUtils.CreateProduct(inventoriesCount: inventoriesCount);

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(command.ProductId, true))
        .Returns(Task.FromResult((Product?)dbProduct));

      _mockProductRepository
        .Setup(m => m.ProduceAsync(
          It.IsAny<ProductTransaction>(),
          It.IsAny<List<InventoryTransaction>>()))
        .Returns(Task.FromResult(true));

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Products.EditedUtcNow));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(command, inventoriesCount);

        _mockUserRepository
        .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());

        _mockProductRepository
        .Verify(m => m.GetOneByIdAsync(command.ProductId, true), Times.Once());

        _mockProductRepository
          .Verify(m => m.ProduceAsync(
            It.IsAny<ProductTransaction>(),
            It.IsAny<List<InventoryTransaction>>()), Times.Once());
      });
    }
  }
}

