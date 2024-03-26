using ErrorOr;
using Florists.Core.DTO.Flowers;
using Florists.Core.DTO.Products;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.Products.Commands.CreateProduct
{
  public record CreateProductCommand(
    string ProductName,
    double UnitPrice,
    string Sku,
    ProductCategoryOptions Category,
    List<RequiredInventoryDTO> RequiredInventories) : IRequest<ErrorOr<ProductResultDTO>>;
}
