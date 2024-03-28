using Florists.Application.Features.Products.Commands.EditProduct;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Application.UnitTests.Products.Commands.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Products;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Products.Commands.EditProduct
{
  public class EditProductCommandTests
  {

    private EditProductCommandHandler _handler;
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
    public async Task HandleEditProductCommand_WhenProductNotFoundById_ShouldReturnProductNotFoundError()
    {
      var command = EditProductCommandUtils.CreateCommand();

      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(command.ProductId, false))
        .Returns(Task.FromResult((Product?)null));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Products.NotFound));

        _mockProductRepository
        .Verify(m => m.GetOneByIdAsync(command.ProductId, false), Times.Once());
      });
    }

    [Test]
    public async Task HandleEditProductCommand_WhenProductWithSameNameFound_ShouldReturnProductAlreadyExistsError()
    {
      var command = EditProductCommandUtils.CreateCommand();
      var dbProduct = EditProductCommandUtils.CreateProduct();

      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(command.ProductId, false))
        .Returns(Task.FromResult((Product?)dbProduct));

      _mockProductRepository
        .Setup(m => m.GetOneByNameAsync(command.ProductName, false))
        .Returns(Task.FromResult((Product?)dbProduct));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Products.AlreadyExists));

        _mockProductRepository
        .Verify(m => m.GetOneByIdAsync(command.ProductId, false), Times.Once());

        _mockProductRepository
        .Verify(m => m.GetOneByNameAsync(command.ProductName, false), Times.Once());
      });
    }

    [Test]
    public async Task HandleEditProductCommand_WhenUnableToEditWithValidCommand_ShouldReturnDatabaseSaveErrorError()
    {
      var command = EditProductCommandUtils.CreateCommand();
      var dbProduct = EditProductCommandUtils.CreateProduct();

      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(command.ProductId, false))
        .Returns(Task.FromResult((Product?)dbProduct));

      _mockProductRepository
        .Setup(m => m.GetOneByNameAsync(command.ProductName, false))
        .Returns(Task.FromResult((Product?)null));

      _mockProductRepository
        .Setup(m => m.UpdateAsync(dbProduct))
        .Returns(Task.FromResult(false));

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Products.EditedUtcNow));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.SaveError));

        _mockProductRepository
        .Verify(m => m.GetOneByIdAsync(command.ProductId, false), Times.Once());

        _mockProductRepository
        .Verify(m => m.GetOneByNameAsync(command.ProductName, false), Times.Once());

        _mockProductRepository
          .Verify(m => m.UpdateAsync(dbProduct), Times.Once());
      });
    }

    [TestCase(1)]
    [TestCase(5)]
    [TestCase(21)]
    [TestCase(3)]
    [TestCase(99)]
    public async Task HandleEditProductCommand_WhenValidCommand_ShouldReturnProductResultDTOWithValidCollection(int inventoriesCount)
    {
      var command = EditProductCommandUtils.CreateCommand(inventoriesCount);
      var dbProduct = EditProductCommandUtils.CreateProduct();

      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(command.ProductId, false))
        .Returns(Task.FromResult((Product?)dbProduct));

      _mockProductRepository
        .Setup(m => m.GetOneByNameAsync(command.ProductName, false))
        .Returns(Task.FromResult((Product?)null));

      _mockProductRepository
        .Setup(m => m.UpdateAsync(dbProduct))
        .Returns(Task.FromResult(true));

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Products.EditedUtcNow));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(command, inventoriesCount);

        _mockProductRepository
        .Verify(m => m.GetOneByIdAsync(command.ProductId, false), Times.Once());

        _mockProductRepository
        .Verify(m => m.GetOneByNameAsync(command.ProductName, false), Times.Once());

        _mockProductRepository
          .Verify(m => m.UpdateAsync(dbProduct), Times.Once());

        _mockDateTimeService
       .Verify(m => m.UtcNow, Times.Exactly(inventoriesCount + 1));
      });
    }
  }
}
