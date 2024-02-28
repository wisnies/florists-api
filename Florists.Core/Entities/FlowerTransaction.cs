using Florists.Core.Enums;

namespace Florists.Core.Entities
{
  public class FlowerTransaction
  {
    public Guid FlowerTransactionId { get; set; }
    public Guid UserId { get; set; }
    public Guid FlowerId { get; set; }
    public string? PurchaseOrderNumber { get; set; }
    public string? ProductionOrderNumber { get; set; }
    public int QuantityBefore { get; set; }
    public int QuantityAfter { get; set; }
    public double TransactionValue { get; set; }
    public FlowerTransactionTypeOptions TransactionType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Flower? Flower { get; set; }
    public FloristsUser? User { get; set; }
  }
}
