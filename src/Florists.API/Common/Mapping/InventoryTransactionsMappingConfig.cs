using Florists.Application.Features.InventoryTransactions.Queries.GetInventoryTransactions;
using Florists.Core.Contracts.InventoryTransactions;
using Florists.Core.Enums;
using Mapster;

namespace Florists.API.Common.Mapping
{
  public class InventoryTransactionsMappingConfig : IRegister
  {
    public void Register(TypeAdapterConfig config)
    {
      config.NewConfig<GetInventoryTransactionsRequest, GetInventoryTransactionsQuery>()
        .Map(dest => dest.Page, src => src.Page <= 0 ? 1 : src.Page)
        .Map(dest => dest.PerPage, src => src.PerPage <= 0 ? (int)PerPageOptions.Ten : src.PerPage);
    }
  }
}
