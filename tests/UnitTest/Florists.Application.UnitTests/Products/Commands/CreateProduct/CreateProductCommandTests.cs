using Florists.Application.Features.Products.Commands.CreateProduct;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Application.UnitTests.Products.Commands.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Products;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Products.Commands.CreateProduct
{
  public class CreateProductCommandTests
  {

    private CreateProductCommandHandler _handler;
    private Mock<IProductRepository> _mockProductRepository;
    private Mock<IDateTimeService> _mockDateTimeService;

    [SetUp]
    public void Setup()
    {
      _mockDateTimeService = new Mock<IDateTimeService>();
      _mockProductRepository = new Mock<IProductRepository>();

      _handler = new(
        _mockProductRepository.Object,
        _mockDateTimeService.Object);
    }

    [Test]
    public async Task HandleCreateProductCommand_WhenProductWithThisNameExists_ShouldReturnProductAlreadyExistsError()
    {
      var command = CreateProductCommandUtils.CreateCommand();
      var dbProduct = CreateProductCommandUtils.CreateProduct();

      _mockProductRepository
        .Setup(m => m.GetOneByNameAsync(command.ProductName, false))
        .Returns(Task.FromResult((Product?)dbProduct));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Products.AlreadyExists));

        _mockProductRepository
        .Verify(m => m.GetOneByNameAsync(command.ProductName, false), Times.Once());
      });
    }

    [Test]
    public async Task HandleCreateProductCommand_WhenUnableToCreateWithValidCommand_ShouldReturnDatabaseSaveErrorError()
    {
      var command = CreateProductCommandUtils.CreateCommand();

      _mockProductRepository
        .Setup(m => m.GetOneByNameAsync(command.ProductName, false))
        .Returns(Task.FromResult((Product?)null));

      _mockProductRepository
        .Setup(m => m.CreateAsync(It.IsAny<Product>()))
        .Returns(Task.FromResult(false));

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Products.UtcNow));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.SaveError));

        _mockProductRepository
        .Verify(m => m.GetOneByNameAsync(command.ProductName, false), Times.Once());

        _mockProductRepository
        .Verify(m => m.CreateAsync(It.IsAny<Product>()), Times.Once());
      });
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(6)]
    [TestCase(99)]
    [TestCase(25)]
    public async Task HandleCreateProductCommand_WhenValidCommand_ShouldReturnProductResultDTOWithValidProductInventoriesCollection(int inventoriesCount)
    {
      var command = CreateProductCommandUtils.CreateCommand(inventoriesCount);

      _mockProductRepository
        .Setup(m => m.GetOneByNameAsync(command.ProductName, false))
        .Returns(Task.FromResult((Product?)null));

      _mockProductRepository
        .Setup(m => m.CreateAsync(It.IsAny<Product>()))
        .Returns(Task.FromResult(true));

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Products.UtcNow));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(command, inventoriesCount);

        _mockProductRepository
        .Verify(m => m.GetOneByNameAsync(command.ProductName, false), Times.Once());

        _mockProductRepository
        .Verify(m => m.CreateAsync(It.IsAny<Product>()), Times.Once());

        _mockDateTimeService
        .Verify(m => m.UtcNow, Times.Exactly(command.RequiredInventories.Count + 1));
      });
    }
  }
}
