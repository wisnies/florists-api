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
    private readonly IInventoryRepository _inventoryRepository;

    public GetFlowersByNameQueryHandler(IInventoryRepository inventoryRepository)
    {
      _inventoryRepository = inventoryRepository;
    }

    public async Task<ErrorOr<InventoriesResultDTO>> Handle(
      GetInventoriesByNameQuery query,
      CancellationToken cancellationToken)
    {
      var offset = query.PerPage * (query.Page - 1);

      var inventories = await _inventoryRepository.GetManyByNameAsync(
        offset,
        query.PerPage,
        query.InventoryName);


      if (inventories is null)
      {
        return CustomErrors.Database.FetchError;
      }

      var count = await _inventoryRepository.CountByNameAsync(query.InventoryName);

      return new InventoriesResultDTO(
        Messages.Database.FetchSuccess,
        count,
        inventories);
    }
  }
}
