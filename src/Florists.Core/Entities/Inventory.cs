using Florists.Core.Enums;

namespace Florists.Core.Entities
{
  public class Inventory
  {
    public Guid InventoryId { get; set; }
    public string InventoryName { get; set; } = string.Empty;
    public int AvailableQuantity { get; set; }
    public double UnitPrice { get; set; }
    public InventoryCategoryOptions Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
