using ErrorOr;
using Florists.Core.DTO.Inventories;
using MediatR;

namespace Florists.Application.Features.Inventories.Queries.GetInventoriesByName
{
  public record GetInventoriesByNameQuery(
    string? InventoryName,
    int Page,
    int PerPage) : IRequest<ErrorOr<InventoriesResultDTO>>;
}
