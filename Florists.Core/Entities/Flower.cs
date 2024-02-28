namespace Florists.Core.Entities
{
  public class Flower
  {
    public Guid FlowerId { get; set; }
    public string FlowerName { get; set; } = string.Empty;
    public int AvailableQuantity { get; set; }
    public double UnitPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
