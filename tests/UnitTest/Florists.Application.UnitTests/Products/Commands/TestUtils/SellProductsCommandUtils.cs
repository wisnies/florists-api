using Florists.Application.Features.Products.Commands.SellProducts;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.DTO.Products;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Products.Commands.TestUtils
{
  public static class SellProductsCommandUtils
  {
    public static SellProductsCommand CreateCommand(int productsCount = 1)
    {
      return new SellProductsCommand(
        Constants.Products.SaleOrderNumber,
        CreateProductsToSell(productsCount),
        Constants.Products.Email);
    }

    public static List<ProductToSellDTO> CreateProductsToSell(int productsCount)
    {
      var productsToSell = new List<ProductToSellDTO>();
      for (int i = 0; i < productsCount; i++)
      {
        var dto = new ProductToSellDTO(
          Guid.Parse(Constants.Products.ProductIdFromIndex(i)),
          i
          );

        productsToSell.Add(dto);
      }
      return productsToSell;
    }

    public static FloristsUser CreateUser()
    {
      return new FloristsUser
      {
        UserId = Guid.Parse(Constants.Products.UserId),
        Email = Constants.Products.Email,
        FirstName = Constants.Products.FirstName,
        LastName = Constants.Products.LastName,
        CreatedAt = DateTime.Parse(Constants.Products.UtcNow),
        Role = new FloristsRole
        {
          RoleId = Guid.Parse(Constants.Products.RoleId),
          UserId = Guid.Parse(Constants.Products.UserId),
          RoleType = Constants.Products.RoleType,
          CreatedAt = DateTime.Parse(Constants.Products.UtcNow),
        }
      };
    }

    public static Product CreateProduct(Guid? productId = null, bool unavailable = false)
    {
      Guid id = Guid.Parse(Constants.Products.ProductId);

      if (productId != null)
      {
        id = (Guid)productId;
      }

      return new Product
      {
        ProductId = id,
        ProductName = Constants.Products.ProductName,
        AvailableQuantity = unavailable ? -1 : Constants.Products.AvailableQuantity,
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
