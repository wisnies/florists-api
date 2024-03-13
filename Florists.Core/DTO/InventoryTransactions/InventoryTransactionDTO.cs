using Florists.Core.DTO.Inventories;
using Florists.Core.DTO.User;
using Florists.Core.Enums;

namespace Florists.Core.DTO.InventoryTransactions
{
  public record InventoryTransactionDTO
  (
    Guid InventoryTransactionId,
    Guid UserId,
    Guid InventoryId,
    string? PurchaseOrderNumber,
    string? ProductionOrderNumber,
    int QuantityBefore,
    int QuantityAfter,
    double TransactionValue,
    InventoryTransactionTypeOptions TransactionType,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    InventoryMetadataDTO? Inventory,
    UserMetadataDTO? User);
}
