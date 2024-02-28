using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Flowers;
using MediatR;

namespace Florists.Application.Features.Flowers.Queries.GetFlowerById
{
  public class GetFlowerByIdQueryHandler : IRequestHandler<GetFlowerByIdQuery, ErrorOr<FlowerResultDTO>>
  {
    private readonly IFlowerRepository _flowerRepository;

    public GetFlowerByIdQueryHandler(IFlowerRepository flowerRepository)
    {
      _flowerRepository = flowerRepository;
    }

    public async Task<ErrorOr<FlowerResultDTO>> Handle(
      GetFlowerByIdQuery query,
      CancellationToken cancellationToken)
    {
      var flower = await _flowerRepository.GetOneByIdAsync(query.FlowerId);

      if (flower is null)
      {
        return CustomErrors.Flowers.FlowerNotFound;
      }

      return new FlowerResultDTO(Messages.Flowers.FetchSuccess, flower);
    }
  }
}
