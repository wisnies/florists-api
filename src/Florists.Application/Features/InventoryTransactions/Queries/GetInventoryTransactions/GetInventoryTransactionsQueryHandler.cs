using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.InventoryTransactions;
using MediatR;

namespace Florists.Application.Features.InventoryTransactions.Queries.GetInventoryTransactions
{
  public class GetInventoryTransactionsQueryHandler : IRequestHandler<GetInventoryTransactionsQuery, ErrorOr<InventoryTransactionsResultDTO>>
  {
    private readonly IInventoryTransactionRepository _inventoryTransactionRepsitory;

    public GetInventoryTransactionsQueryHandler(IInventoryTransactionRepository inventoryTransactionRepsitory)
    {
      _inventoryTransactionRepsitory = inventoryTransactionRepsitory;
    }

    public async Task<ErrorOr<InventoryTransactionsResultDTO>> Handle(
      GetInventoryTransactionsQuery query,
      CancellationToken cancellationToken)
    {
      var offset = query.PerPage * (query.Page - 1);
      var transactions = await _inventoryTransactionRepsitory.GetManyAsync(
        offset,
        query.PerPage,
        query.DateFrom,
        query.DateTo,
        query.TransactionType,
        query.OrderBy,
        query.Order);

      if (transactions is null)
      {
        return CustomErrors.Database.FetchError;
      }

      var count = await _inventoryTransactionRepsitory.CountAsync(
        query.DateFrom,
        query.DateTo,
        query.TransactionType);

      return new InventoryTransactionsResultDTO(
        Messages.Database.FetchSuccess,
        count,
        transactions);
    }
  }
}
