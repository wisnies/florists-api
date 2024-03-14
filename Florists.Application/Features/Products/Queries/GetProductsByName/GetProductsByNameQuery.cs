using ErrorOr;
using Florists.Core.DTO.Products;
using MediatR;

namespace Florists.Application.Features.Products.Queries.GetProductsByName
{
  public record GetProductsByNameQuery(
    string? ProductName,
    int Page,
    int PerPage) : IRequest<ErrorOr<ProductsResultDTO>>;
}
