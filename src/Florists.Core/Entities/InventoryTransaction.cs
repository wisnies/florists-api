using Florists.Core.Enums;

namespace Florists.Core.Entities
{
  public class InventoryTransaction
  {
    public Guid InventoryTransactionId { get; set; }
    public Guid UserId { get; set; }
    public Guid InventoryId { get; set; }
    public string? PurchaseOrderNumber { get; set; }
    public string? ProductionOrderNumber { get; set; }
    public int QuantityBefore { get; set; }
    public int QuantityAfter { get; set; }
    public double TransactionValue { get; set; }
    public InventoryTransactionTypeOptions TransactionType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Inventory? Inventory { get; set; }
    public FloristsUser? User { get; set; }
  }
}
