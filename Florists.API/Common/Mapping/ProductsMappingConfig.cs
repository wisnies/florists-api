using Florists.Application.Features.Inventories.Commands.EditInventory;
using Florists.Core.Contracts.Inventories;
using Mapster;

namespace Florists.API.Common.Mapping
{
  public class ProductsMappingConfig : IRegister
  {
    public void Register(TypeAdapterConfig config)
    {
      config.NewConfig<(EditInventoryRequest request, Guid inventoryId), EditInventoryCommand>()
        .Map(dest => dest.InventoryId, src => src.inventoryId)
        .Map(dest => dest, src => src.request);
    }
  }
}
