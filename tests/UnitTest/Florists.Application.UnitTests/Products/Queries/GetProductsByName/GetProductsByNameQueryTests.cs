using Florists.Application.Features.Products.Queries.GetProductsByName;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.UnitTests.Products.Queries.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Products;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Products.Queries.GetProductsByName
{
  public class GetProductsByNameTests
  {
    private GetProductsByNameQueryHandler _handler;
    private Mock<IProductRepository> _mockProductRepository;

    [SetUp]
    public void SetUp()
    {
      _mockProductRepository = new Mock<IProductRepository>();
      _handler = new(_mockProductRepository.Object);
    }

    [TestCase(1, 1)]
    [TestCase(2, 2)]
    [TestCase(5, 1)]
    [TestCase(3, 10)]
    [TestCase(123, 1)]
    public async Task HandleGetProductsByName_WhenValidQuery_ShouldReturnProductsResultDTOWithValidCollectionLength(int perPage, int page)
    {
      var query = GetProductsByNameQueryUtils.CreateQuery(perPage, page);
      var offset = query.PerPage * (query.Page - 1);
      var dbProducts = GetProductsByNameQueryUtils.CreateProducts(offset, perPage);


      _mockProductRepository
        .Setup(m => m.GetManyByNameAsync(
          offset,
          query.PerPage,
          query.ProductName))
        .Returns(Task.FromResult((List<Product>?)dbProducts));

      _mockProductRepository
        .Setup(m => m.CountByNameAsync(query.ProductName))
        .Returns(Task.FromResult(Constants.Products.ProductsCount));

      var result = await _handler.Handle(
        query,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(query, offset);

        _mockProductRepository
        .Verify(m => m.GetManyByNameAsync(
          offset,
          query.PerPage,
          query.ProductName),
          Times.Once());

        _mockProductRepository
        .Verify(m => m.CountByNameAsync(query.ProductName),
          Times.Once());
      });
    }

    [Test]
    public async Task HandleGetInventoriesByName_WhenUnableToFetchWithValidQuery_ShouldReturnDatabaseFetchError()
    {
      var query = GetProductsByNameQueryUtils.CreateQuery();

      var offset = query.PerPage * (query.Page - 1);

      _mockProductRepository
        .Setup(m => m.GetManyByNameAsync(
          offset,
          query.PerPage,
          query.ProductName))
        .Returns(Task.FromResult((List<Product>?)null));

      var result = await _handler.Handle(
        query,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.FetchError));

        _mockProductRepository
        .Verify(m => m.GetManyByNameAsync(
          offset,
          query.PerPage,
          query.ProductName),
          Times.Once());
      });
    }
  }
}
