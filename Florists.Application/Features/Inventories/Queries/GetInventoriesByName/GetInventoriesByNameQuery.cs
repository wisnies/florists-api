using ErrorOr;
using Florists.Core.DTO.Inventories;
using MediatR;

namespace Florists.Application.Features.Inventories.Queries.GetInventoriesByName
{
  public record GetInventoriesByNameQuery(
    string InventoryName,
    int Page = 1,
    int PerPage = 10) : IRequest<ErrorOr<InventoriesResultDTO>>;
}
