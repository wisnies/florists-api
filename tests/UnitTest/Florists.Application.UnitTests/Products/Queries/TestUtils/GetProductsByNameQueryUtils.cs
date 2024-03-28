using Florists.Application.Features.Products.Queries.GetProductsByName;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Products.Queries.TestUtils
{
  public static class GetProductsByNameQueryUtils
  {
    public static GetProductsByNameQuery CreateQuery(int perPage = 10, int page = 1)
    {
      return new GetProductsByNameQuery(
        Constants.Products.ProductName,
        page,
        perPage);
    }

    public static List<Product> CreateProducts(int offset, int perPage)
    {
      var products = new List<Product>();

      var total = perPage >= Constants.Products.ProductsCount ?
        Constants.Products.ProductsCount :
        perPage;
      total += offset;

      for (int i = offset; i < total; i++)
      {
        var product = new Product
        {
          ProductId = Guid.NewGuid(),
          ProductName = Constants.Products.ProductNameFromIndex(i),
          Sku = Constants.Products.Sku,
          AvailableQuantity = Constants.Products.AvailableQuantity,
          UnitPrice = Constants.Products.UnitPrice,
          Category = Constants.Products.Category,
        };
        products.Add(product);
      }

      return products;
    }
  }
}
