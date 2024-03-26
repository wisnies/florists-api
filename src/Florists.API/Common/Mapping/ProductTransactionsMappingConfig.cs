using Florists.Application.Features.ProductTransactions.Queries.GetProductTransactions;
using Florists.Core.Contracts.ProductTransactions;
using Florists.Core.Enums;
using Mapster;

namespace Florists.API.Common.Mapping
{
  public class ProductTransactionsMappingConfig : IRegister
  {
    public void Register(TypeAdapterConfig config)
    {
      config.NewConfig<GetProductTransactionsRequest, GetProductTransactionsQuery>()
        .Map(dest => dest.Page, src => src.Page <= 0 ? 1 : src.Page)
        .Map(dest => dest.PerPage, src => src.PerPage <= 0 ? (int)PerPageOptions.Ten : src.PerPage);
    }
  }
}
