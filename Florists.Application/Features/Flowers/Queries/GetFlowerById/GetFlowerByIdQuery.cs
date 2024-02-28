using ErrorOr;
using Florists.Core.DTO.Flowers;
using MediatR;

namespace Florists.Application.Features.Flowers.Queries.GetFlowerById
{
  public record GetFlowerByIdQuery(
    Guid FlowerId) : IRequest<ErrorOr<FlowerResultDTO>>;
}
