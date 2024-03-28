using Florists.Application.Features.Products.Commands.DeleteProduct;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Products.Commands.TestUtils
{
  public static class DeleteProductCommandUtils
  {
    public static DeleteProductCommand CreateCommand()
    {
      return new DeleteProductCommand(Guid.Parse(Constants.Products.ProductId));
    }

    public static Product CreateProduct()
    {
      return new Product
      {
        ProductId = Guid.Parse(Constants.Products.ProductId),
        ProductName = Constants.Products.ProductName,
        AvailableQuantity = Constants.Products.AvailableQuantity,
        UnitPrice = Constants.Products.UnitPrice,
        Sku = Constants.Products.Sku,
        IsActive = true,
        Category = Constants.Products.Category,
        CreatedAt = DateTime.Parse(Constants.Products.UtcNow),
        UpdatedAt = DateTime.Parse(Constants.Products.UtcNow),
      };
    }
  }
}
