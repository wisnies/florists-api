using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.ProductTransactions;
using MediatR;

namespace Florists.Application.Features.ProductTransactions.Queries.GetProductTransactions
{
  public class GetProductTransactionsQueryHandler : IRequestHandler<GetProductTransactionsQuery, ErrorOr<ProductTransactionsResultDTO>>
  {
    private readonly IProductTransactionRepository _productTransactionRepository;

    public GetProductTransactionsQueryHandler(IProductTransactionRepository productTransactionRepository)
    {
      _productTransactionRepository = productTransactionRepository;
    }

    public async Task<ErrorOr<ProductTransactionsResultDTO>> Handle(
      GetProductTransactionsQuery query,
      CancellationToken cancellationToken)
    {
      var offset = query.PerPage * (query.Page - 1);
      var transactions = await _productTransactionRepository.GetManyAsync(
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

      var count = await _productTransactionRepository.CountAsync(
        query.DateFrom,
        query.DateTo,
        query.TransactionType);

      return new ProductTransactionsResultDTO(
        Messages.Database.FetchSuccess,
        count,
        transactions);
    }
  }
}
