using Florists.Application.Features.Products.Commands.EditProduct;
using Florists.Application.Features.Products.Commands.ProduceProduct;
using Florists.Application.Features.Products.Commands.SellProducts;
using Florists.Core.Contracts.Products;
using Mapster;

namespace Florists.API.Common.Mapping
{
  public class ProductsMappingConfig : IRegister
  {
    public void Register(TypeAdapterConfig config)
    {
      config.NewConfig<(EditProductRequest request, Guid productId), EditProductCommand>()
        .Map(dest => dest.ProductId, src => src.productId)
        .Map(dest => dest, src => src.request);

      config.NewConfig<(SellProductsRequest request, string email), SellProductsCommand>()
        .Map(dest => dest.Email, src => src.email)
        .Map(dest => dest, src => src.request);

      config.NewConfig<(ProduceProductRequest request, string email), ProduceProductCommand>()
        .Map(dest => dest.Email, src => src.email)
        .Map(dest => dest, src => src.request);
    }
  }
}
