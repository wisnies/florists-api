using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.InventoryTransactions.Queries.GetInventoryTransactions
{
  public class GetInventoryTransactionsQueryValidator : AbstractValidator<GetInventoryTransactionsQuery>
  {
    public GetInventoryTransactionsQueryValidator()
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
