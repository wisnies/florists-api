using Florists.Core.Enums;

namespace Florists.Core.Entities
{
  public class ProductTransaction
  {
    public Guid ProductTransactionId { get; set; }
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public string? SaleOrderNumber { get; set; }
    public string? ProductionOrderNumber { get; set; }
    public int QuantityBefore { get; set; }
    public int QuantityAfter { get; set; }
    public double TransactionValue { get; set; }
    public ProductTransactionOptions TransactionType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Product? Product { get; set; }
    public FloristsUser? User { get; set; }
  }
}
