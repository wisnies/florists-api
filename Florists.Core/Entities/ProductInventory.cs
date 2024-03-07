namespace Florists.Core.Entities
{
  public class ProductInventory
  {
    public Guid ProductId { get; set; }
    public Guid InventoryId { get; set; }
    public int RequiredQuantity { get; set; }
    public Product? Product { get; set; }
    public Inventory? Inventory { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
