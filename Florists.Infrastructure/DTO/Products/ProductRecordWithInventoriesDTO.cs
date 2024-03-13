using Florists.Core.Enums;

namespace Florists.Infrastructure.DTO.Products
{
  public class ProductRecordWithInventoryDTO
  {
    public Guid product_id { get; set; }
    public string product_name { get; set; } = string.Empty;
    public int product_available_quantity { get; set; }
    public double product_unit_price { get; set; }
    public string sku { get; set; } = string.Empty;
    public bool is_active { get; set; }
    public ProductCategoryOptions product_category { get; set; }
    public DateTime product_created_at { get; set; }
    public DateTime product_updated_at { get; set; }
    public Guid inventory_id { get; set; }
    public string inventory_name { get; set; } = string.Empty;
    public int inventory_available_quantity { get; set; }
    public double inventory_unit_price { get; set; }
    public InventoryCategoryOptions inventory_category { get; set; }
    public DateTime inventory_created_at { get; set; }
    public DateTime inventory_updated_at { get; set; }
    public int required_quantity { get; set; }
  }
}
