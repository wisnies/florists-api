using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Inventories;
using MediatR;

namespace Florists.Application.Features.Inventories.Queries.GetInventoriesByName
{
  public class GetFlowersByNameQueryHandler : IRequestHandler<GetInventoriesByNameQuery, ErrorOr<InventoriesResultDTO>>
  {
    private readonly IInventoryRepository _invenotryRepository;

    public GetFlowersByNameQueryHandler(IInventoryRepository invenotryRepository)
    {
      _invenotryRepository = invenotryRepository;
    }

    public async Task<ErrorOr<InventoriesResultDTO>> Handle(
      GetInventoriesByNameQuery query,
      CancellationToken cancellationToken)
    {
      var offset = query.PerPage * (query.Page - 1);

      var inventories = await _invenotryRepository.GetManyByNameAsync(
        query.InventoryName,
        offset,
        query.PerPage);


      if (inventories is null)
      {
        return CustomErrors.Database.FetchError;
      }

      var count = await _invenotryRepository.CountByNameAsync(query.InventoryName);

      return new InventoriesResultDTO(
        Messages.Database.FetchSuccess,
        count,
        inventories);
    }
  }
}
