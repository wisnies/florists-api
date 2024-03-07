﻿namespace Florists.Infrastructure.Settings
{
  public class MySqlSettings
  {
    public const string SectionName = "MySqlSettings";
    public string ConnectionString { get; set; } = null!;
    public string UsersTable { get; set; } = null!;
    public string RolesTable { get; set; } = null!;
    public string InventoriesTable { get; set; } = null!;
    public string InventoryTransactionsTable { get; set; } = null!;
    public string ProductsTable { get; set; } = string.Empty;
    public string ProductInventoriesTable { get; set; } = string.Empty;
  }
}
