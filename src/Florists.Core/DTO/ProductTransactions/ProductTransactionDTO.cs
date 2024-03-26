using Florists.Core.DTO.Products;
using Florists.Core.DTO.User;
using Florists.Core.Enums;

namespace Florists.Core.DTO.ProductTransactions
{
  public record ProductTransactionDTO
  (
    Guid ProductTransactionId,
    Guid UserId,
    Guid ProductId,
    string? SaleOrderNumber,
    string? ProductionOrderNumber,
    int QuantityBefore,
    int QuantityAfter,
    double TransactionValue,
    ProductTransactionTypeOptions TransactionType,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    ProductTransactionDataDTO? Product,
    UserTransactionDataDTO? User);
}
