using Florists.Application.Features.Flowers.Commands.EditFlower;
using Florists.Application.Features.Flowers.Commands.PurchaseFlowers;
using Florists.Core.Contracts.Flowers;
using Mapster;

namespace Florists.API.Common.Mapping
{
  public class FlowersMappingCOnfig : IRegister
  {
    public void Register(TypeAdapterConfig config)
    {
      config.NewConfig<(EditFlowerRequest request, Guid flowerId), EditFlowerCommand>()
        .Map(dest => dest.FlowerId, src => src.flowerId)
        .Map(dest => dest, src => src.request);

      config.NewConfig<(PurchaseFlowersRequest request, string email), PurchaseFlowersCommand>()
        .Map(dest => dest.Email, src => src.email)
        .Map(dest => dest, src => src.request);
    }
  }
}
