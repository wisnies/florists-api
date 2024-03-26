using Florists.Core.Enums;

namespace Florists.Infrastructure.DTO.InventoryTransactions
{
  public class InventoryTransactionRecordWithUserAndInventoryDTO
  {
    public Guid transaction_id { get; set; }
    public Guid inventory_id { get; set; }
    public Guid user_id { get; set; }
    public string purchase_order_number { get; set; } = string.Empty;
    public string production_order_number { get; set; } = string.Empty;
    public int quantity_before { get; set; }
    public int quantity_after { get; set; }
    public double transaction_value { get; set; }
    public InventoryTransactionTypeOptions transaction_type { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string first_name { get; set; } = string.Empty;
    public string last_name { get; set; } = string.Empty;
    public string inventory_name { get; set; } = string.Empty;
    public double unit_price { get; set; }
    public InventoryCategoryOptions category { get; set; }
  }
}
