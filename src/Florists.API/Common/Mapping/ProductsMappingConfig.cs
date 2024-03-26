using Florists.Application.Features.Products.Commands.EditProduct;
using Florists.Application.Features.Products.Commands.ProduceProduct;
using Florists.Application.Features.Products.Commands.SellProducts;
using Florists.Application.Features.Products.Queries.GetProductsByName;
using Florists.Core.Contracts.Products;
using Florists.Core.Enums;
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

      config.NewConfig<GetProductsByNameRequest, GetProductsByNameQuery>()
        .Map(dest => dest.Page, src => src.Page <= 0 ? 1 : src.Page)
        .Map(dest => dest.PerPage, src => src.PerPage <= 0 ? (int)PerPageOptions.Ten : src.PerPage);
    }
  }
}
