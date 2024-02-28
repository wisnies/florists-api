using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Flowers;
using MediatR;

namespace Florists.Application.Features.Flowers.Queries.GetFlowersByName
{
  public class GetFlowersByNameQueryHandler : IRequestHandler<GetFlowersByNameQuery, ErrorOr<FlowersResultDTO>>
  {
    private readonly IFlowerRepository _flowerRepository;

    public GetFlowersByNameQueryHandler(IFlowerRepository flowerRepository)
    {
      _flowerRepository = flowerRepository;
    }

    public async Task<ErrorOr<FlowersResultDTO>> Handle(
      GetFlowersByNameQuery query,
      CancellationToken cancellationToken)
    {
      var offset = query.PerPage * (query.Page - 1);

      var flowers = await _flowerRepository.GetManyByNameAsync(query.FlowerName, offset, query.PerPage);

      if (flowers is null)
      {
        return CustomErrors.Flowers.UnableToFetchFlowers;
      }

      return new FlowersResultDTO(Messages.Flowers.FetchSuccess, flowers);
    }
  }
}
