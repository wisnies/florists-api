using ErrorOr;
using Florists.Core.DTO.Flowers;
using Florists.Core.DTO.Products;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.Products.Commands.EditProduct
{
  public record EditProductCommand(
    Guid ProductId,
    string ProductName,
    double UnitPrice,
    string Sku,
    ProductCategoryOptions Category,
    List<RequiredInventoryDTO> RequiredInventories) : IRequest<ErrorOr<ProductResultDTO>>;
}
