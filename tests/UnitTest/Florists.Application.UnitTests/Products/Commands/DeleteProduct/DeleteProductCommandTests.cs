using Florists.Application.Features.Products.Commands.DeleteProduct;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Application.UnitTests.Products.Commands.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Products;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Products.Commands.DeleteProduct
{
  public class DeleteProductCommandTests
  {
    private DeleteProductCommandHandler _handler;
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
    public async Task HandleDeleteProductCommand_WhenProductNotFoundById_ShouldReturnProductNotFoundError()
    {
      var command = DeleteProductCommandUtils.CreateCommand();

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
    public async Task HandleDeleteProductCommand_WhenUnableToDeleteWithValidCommand_ShouldReturnDatabaseSaveError()
    {
      var command = DeleteProductCommandUtils.CreateCommand();
      var dbProduct = DeleteProductCommandUtils.CreateProduct();

      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(command.ProductId, false))
        .Returns(Task.FromResult((Product?)dbProduct));

      _mockProductRepository
        .Setup(m => m.SoftDeleteAsync(It.IsAny<Product>()))
        .Returns(Task.FromResult(false));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.SaveError));

        _mockProductRepository
        .Verify(m => m.GetOneByIdAsync(command.ProductId, false), Times.Once());

        _mockProductRepository
          .Verify(m => m.SoftDeleteAsync(It.IsAny<Product>()), Times.Once());
      });
    }

    [Test]
    public async Task HandleDeleteProductCommand_WhenValidCommand_ShouldReturnProductResultDTO()
    {
      var command = DeleteProductCommandUtils.CreateCommand();
      var dbProduct = DeleteProductCommandUtils.CreateProduct();

      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(command.ProductId, false))
        .Returns(Task.FromResult((Product?)dbProduct));

      _mockProductRepository
        .Setup(m => m.SoftDeleteAsync(dbProduct))
        .Returns(Task.FromResult(true));

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Products.EditedUtcNow));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(command);

        _mockProductRepository
        .Verify(m => m.GetOneByIdAsync(command.ProductId, false), Times.Once());

        _mockProductRepository
          .Verify(m => m.SoftDeleteAsync(It.IsAny<Product>()), Times.Once());

        _mockDateTimeService
        .Verify(m => m.UtcNow, Times.Once());
      });
    }
  }
}
