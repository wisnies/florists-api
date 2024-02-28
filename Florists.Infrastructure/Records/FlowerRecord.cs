namespace Florists.Infrastructure.Records
{
  public class FlowerRecord
  {
    public Guid flower_id { get; set; }
    public string flower_name { get; set; } = string.Empty;
    public int available_quantity { get; set; }
    public double unit_price { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
  }
}
