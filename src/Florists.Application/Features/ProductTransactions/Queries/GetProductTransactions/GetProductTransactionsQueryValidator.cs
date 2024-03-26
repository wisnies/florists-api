using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.ProductTransactions.Queries.GetProductTransactions
{
  public class GetProductTransactionsQueryValidator : AbstractValidator<GetProductTransactionsQuery>
  {
    public GetProductTransactionsQueryValidator()
    {
      RuleFor(x => x.OrderBy)
        .IsInEnum()
        .WithMessage(Messages.Transactions.InvalidOrderBy);

      RuleFor(x => x.Order)
        .IsInEnum()
        .WithMessage(Messages.Transactions.InvalidOrder);
    }
  }
}
