using Florists.Core.Enums;

namespace Florists.Infrastructure.DTO.Inventories
{
  public class InventoryRecordDTO
  {
    public Guid inventory_id { get; set; }
    public string inventory_name { get; set; } = string.Empty;
    public int available_quantity { get; set; }
    public double unit_price { get; set; }
    public InventoryCategoryOptions category { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
  }
}
