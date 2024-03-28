using Florists.Application.Features.Products.Commands.SellProducts;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Application.UnitTests.Products.Commands.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Products;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Products.Commands.SellProduct
{
  public class SellProductCommandTests
  {
    private SellProductsCommandHandler _handler;
    private Mock<IProductRepository> _mockProductRepository;
    private Mock<IProductTransactionRepository> _mockProductTransactionRepository;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IDateTimeService> _mockDateTimeService;

    [SetUp]
    public void Setup()
    {
      _mockDateTimeService = new Mock<IDateTimeService>();
      _mockProductRepository = new Mock<IProductRepository>();
      _mockProductTransactionRepository = new Mock<IProductTransactionRepository>();
      _mockUserRepository = new Mock<IUserRepository>();

      _handler = new(
        _mockProductRepository.Object,
        _mockProductTransactionRepository.Object,
        _mockDateTimeService.Object,
        _mockUserRepository.Object);
    }

    [Test]
    public async Task HandleSellProductsCommand_WhenUserNotFoundByEmail_ShouldReturnUserNotFoundError()
    {
      var command = SellProductsCommandUtils.CreateCommand();

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
    public async Task HandleSellProductsCommand_WhenProductIsNotFound_ShouldReturnProductNotFoundError()
    {
      var command = SellProductsCommandUtils.CreateCommand();
      var dbUser = SellProductsCommandUtils.CreateUser();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(It.IsAny<Guid>(), false))
        .Returns(Task.FromResult((Product?)null));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Products.NotFound));

        _mockUserRepository
          .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());

        _mockProductRepository
        .Verify(m => m.GetOneByIdAsync(It.IsAny<Guid>(), false), Times.AtLeastOnce());
      });
    }

    [Test]
    public async Task HandleSellProductsCommand_WhenProductQuantityIsInvalid_ShouldReturnProductNotFoundError()
    {
      var command = SellProductsCommandUtils.CreateCommand();
      var dbUser = SellProductsCommandUtils.CreateUser();
      var dbProduct = SellProductsCommandUtils.CreateProduct();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(It.IsAny<Guid>(), false))
        .Returns(Task.FromResult((Product?)null));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Products.NotFound));

        _mockUserRepository
          .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());

        _mockProductRepository
        .Verify(m => m.GetOneByIdAsync(It.IsAny<Guid>(), false), Times.AtLeastOnce());
      });
    }

    [Test]
    public async Task HandleSellProductsCommand_WhenUnableToSellProductsWithValidCommand_ShouldReturnProductQuantityToSellUnavailableError()
    {
      var command = SellProductsCommandUtils.CreateCommand();
      var dbUser = SellProductsCommandUtils.CreateUser();
      var dbProduct = SellProductsCommandUtils.CreateProduct(null, true);

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(It.IsAny<Guid>(), false))
        .Returns(Task.FromResult((Product?)dbProduct));


      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Products.QuantityToSellUnavailable(dbProduct.ProductName)));

        _mockUserRepository
          .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());

        _mockProductRepository
        .Verify(m => m.GetOneByIdAsync(
          It.IsAny<Guid>(), false),
          Times.Exactly(command.ProductsToSell.Count));
      });
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(23)]
    [TestCase(17)]
    [TestCase(8)]
    public async Task HandleSellProductsCommand_WhenValidCommand_ShouldReturnProductTransactionsResultDTOWithValidCollectionLength(int count)
    {
      var command = SellProductsCommandUtils.CreateCommand(count);
      var dbUser = SellProductsCommandUtils.CreateUser();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      foreach (var dto in command.ProductsToSell)
      {
        var dbProduct = SellProductsCommandUtils.CreateProduct(dto.ProductId);
        _mockProductRepository
          .Setup(m => m.GetOneByIdAsync(dto.ProductId, false))
          .Returns(Task.FromResult((Product?)dbProduct));
      }

      _mockProductTransactionRepository
        .Setup(m => m.SellAsync(It.IsAny<List<ProductTransaction>>()))
        .Returns(Task.FromResult((true)));

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Inventories.EditedUtcNow));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(command, count);

        _mockUserRepository
          .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());

        _mockProductRepository
        .Verify(m => m.GetOneByIdAsync(
          It.IsAny<Guid>(), false),
          Times.Exactly(command.ProductsToSell.Count));

        _mockProductTransactionRepository
        .Verify(m => m.SellAsync(It.IsAny<List<ProductTransaction>>()), Times.Once());

        _mockDateTimeService
        .Verify(m => m.UtcNow, Times.Exactly(count * 2));
      });
    }
  }
}
