using Florists.Core.Enums;

namespace Florists.Infrastructure.DTO.ProductTransactions
{
  public class ProductTransactionRecordWithUserAndProductDTO
  {
    public Guid transaction_id { get; set; }
    public Guid product_id { get; set; }
    public Guid user_id { get; set; }
    public string sale_order_number { get; set; } = string.Empty;
    public string production_order_number { get; set; } = string.Empty;
    public int quantity_before { get; set; }
    public int quantity_after { get; set; }
    public double transaction_value { get; set; }
    public ProductTransactionTypeOptions transaction_type { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string first_name { get; set; } = string.Empty;
    public string last_name { get; set; } = string.Empty;
    public string product_name { get; set; } = string.Empty;
    public double unit_price { get; set; }
    public ProductCategoryOptions category { get; set; }
  }
}
