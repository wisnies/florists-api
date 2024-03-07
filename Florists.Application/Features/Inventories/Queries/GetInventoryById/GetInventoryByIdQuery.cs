using ErrorOr;
using Florists.Core.DTO.Inventories;
using MediatR;

namespace Florists.Application.Features.Inventories.Queries.GetInventoryById
{
  public record GetInventoryByIdQuery(
    Guid InventoryId) : IRequest<ErrorOr<InventoryResultDTO>>;
}
