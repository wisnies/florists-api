using Florists.Application.Features.Products.Queries.GetProductById;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.UnitTests.Products.Queries.TestUtils;
using Florists.Application.UnitTests.TestUtils.Extenstions.Products;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Products.Queries.GetProductById
{
  public class GetProductByIdQueryTests
  {
    private GetProductByIdQueryHandler _handler;
    private Mock<IProductRepository> _mockProductRepository;

    [SetUp]
    public void Setup()
    {
      _mockProductRepository = new Mock<IProductRepository>();
      _handler = new(_mockProductRepository.Object);
    }


    [Test]
    public async Task HandleGetProductByIdQuery_WhenIdIsInvalid_ShouldReturnProductNotFoundError()
    {
      var query = GetProductByNameQueryUtils.CreateQuery();

      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(query.ProductId, true))
        .Returns(Task.FromResult((Product?)null));

      var result = await _handler.Handle(query, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(
          result.FirstError,
          Is.EqualTo(CustomErrors.Products.NotFound));

        _mockProductRepository.Verify(
          m => m.GetOneByIdAsync(query.ProductId, true),
          Times.Once());
      });
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(7)]
    [TestCase(3)]
    [TestCase(99)]
    public async Task HandleGetProductByIdQuery_WhenIdIsValid_ShouldReturnProductResultDTOWithValidCollectionLength(int inventoriesCount)
    {
      var query = GetProductByNameQueryUtils.CreateQuery();
      var dbProduct = GetProductByNameQueryUtils.CreateProduct(inventoriesCount);


      _mockProductRepository
        .Setup(m => m.GetOneByIdAsync(query.ProductId, true))
        .Returns(Task.FromResult((Product?)dbProduct));

      var result = await _handler.Handle(query, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(query, inventoriesCount);

        _mockProductRepository.Verify(
          m => m.GetOneByIdAsync(query.ProductId, true),
          Times.Once());
      });
    }
  }
}
