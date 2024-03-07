using ErrorOr;
using Florists.Core.DTO.Products;
using MediatR;

namespace Florists.Application.Features.Products.Queries.GetProductById
{
  public record GetProductByIdQuery(Guid ProductId) : IRequest<ErrorOr<ProductResultDTO>>;
}
