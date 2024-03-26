using ErrorOr;
using Florists.Core.DTO.InventoryTransactions;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.InventoryTransactions.Queries.GetInventoryTransactions
{
  public record GetInventoryTransactionsQuery(
    DateTime? DateFrom,
    DateTime? DateTo,
    InventoryTransactionTypeOptions? TransactionType,
    OrderOptions? Order,
    TransactionsOrderByOptions? OrderBy,
    int Page,
    int PerPage) : IRequest<ErrorOr<InventoryTransactionsResultDTO>>;
}
