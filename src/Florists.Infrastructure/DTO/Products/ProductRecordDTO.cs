using Florists.Core.Enums;

namespace Florists.Infrastructure.DTO.Products
{
  public class ProductRecordDTO
  {
    public Guid product_id { get; set; }
    public string product_name { get; set; } = string.Empty;
    public int available_quantity { get; set; }
    public double unit_price { get; set; }
    public string sku { get; set; } = string.Empty;
    public bool is_active { get; set; }
    public ProductCategoryOptions category { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
  }
}
