using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Inventories;
using MediatR;

namespace Florists.Application.Features.Inventories.Queries.GetInventoryById
{
  public class GetInventoryByIdQueryHandler : IRequestHandler<GetInventoryByIdQuery, ErrorOr<InventoryResultDTO>>
  {
    private readonly IInventoryRepository _inventoryRepository;

    public GetInventoryByIdQueryHandler(IInventoryRepository invenotryRepository)
    {
      _inventoryRepository = invenotryRepository;
    }

    public async Task<ErrorOr<InventoryResultDTO>> Handle(
      GetInventoryByIdQuery query,
      CancellationToken cancellationToken)
    {
      var inventory = await _inventoryRepository.GetOneByIdAsync(query.InventoryId);

      if (inventory is null)
      {
        return CustomErrors.Inventories.NotFound;
      }

      return new InventoryResultDTO(Messages.Database.FetchSuccess, inventory);
    }
  }
}
