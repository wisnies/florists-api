using Florists.Application.Features.Inventories.Commands.EditInventory;
using Florists.Application.Features.Inventories.Commands.PurchaseInventories;
using Florists.Core.Contracts.Inventories;
using Mapster;

namespace Florists.API.Common.Mapping
{
  public class InventoriesMappingConfig : IRegister
  {
    public void Register(TypeAdapterConfig config)
    {
      config.NewConfig<(EditInventoryRequest request, Guid inventoryId), EditInventoryCommand>()
        .Map(dest => dest.InventoryId, src => src.inventoryId)
        .Map(dest => dest, src => src.request);

      config.NewConfig<(PurchaseInventoriesRequest request, string email), PurchaseInventoriesCommand>()
        .Map(dest => dest.Email, src => src.email)
        .Map(dest => dest, src => src.request);
    }
  }
}
