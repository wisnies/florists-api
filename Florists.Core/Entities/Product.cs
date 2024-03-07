using Florists.Core.Enums;

namespace Florists.Core.Entities
{
  public class Product
  {
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int AvailableQuantity { get; set; } = 0;
    public double UnitPrice { get; set; }
    public string Sku { get; set; } = string.Empty;
    public ProductCategoryOptions Category { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<ProductInventory>? ProductInventories { get; set; }
  }
}
