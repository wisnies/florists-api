using ErrorOr;
using Florists.Core.DTO.Flowers;
using MediatR;

namespace Florists.Application.Features.Flowers.Queries.GetFlowersByName
{
  public record GetFlowersByNameQuery(
    string FlowerName,
    int Page = 1,
    int PerPage = 10) : IRequest<ErrorOr<FlowersResultDTO>>;
}
