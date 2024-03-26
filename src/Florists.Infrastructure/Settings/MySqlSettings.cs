namespace Florists.Infrastructure.Settings
{
  public class MySqlSettings
  {
    public const string SectionName = "MySqlSettings";
    public string ConnectionString { get; set; } = string.Empty;
    public string UsersTable { get; set; } = string.Empty;
    public string RolesTable { get; set; } = string.Empty;
    public string InventoriesTable { get; set; } = string.Empty;
    public string InventoryTransactionsTable { get; set; } = string.Empty;
    public string ProductsTable { get; set; } = string.Empty;
    public string ProductInventoriesTable { get; set; } = string.Empty;
    public string ProductTransactionsTable { get; set; } = string.Empty;
  }
}
