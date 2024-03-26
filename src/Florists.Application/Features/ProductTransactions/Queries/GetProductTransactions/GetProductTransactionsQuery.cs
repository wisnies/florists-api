using ErrorOr;
using Florists.Core.DTO.ProductTransactions;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.ProductTransactions.Queries.GetProductTransactions
{
  public record GetProductTransactionsQuery(
    DateTime? DateFrom,
    DateTime? DateTo,
    ProductTransactionTypeOptions? TransactionType,
    OrderOptions? Order,
    TransactionsOrderByOptions? OrderBy,
    int Page,
    int PerPage) : IRequest<ErrorOr<ProductTransactionsResultDTO>>;
}
