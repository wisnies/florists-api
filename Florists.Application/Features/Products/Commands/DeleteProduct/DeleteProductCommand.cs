using ErrorOr;
using Florists.Core.DTO.Products;
using MediatR;

namespace Florists.Application.Features.Products.Commands.DeleteProduct
{
  public record DeleteProductCommand(Guid ProductId) : IRequest<ErrorOr<ProductResultDTO>>;
}
