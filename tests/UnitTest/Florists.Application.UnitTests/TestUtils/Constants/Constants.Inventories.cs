using Florists.Core.Enums;

namespace Florists.Application.UnitTests.TestUtils.Constants
{
  public static partial class Constants
  {
    public static class Inventories
    {
      public const string InventoryId = "03950F56-120D-4A2F-8418-1978ED8BEE15";
      public const string OtherInventoryId = "9A6AAA04-4636-41E2-B4FE-A4FB4EB174F7";
      public const string InventoryName = "Test Inventory";
      public const string EditedInventoryName = "Edited Test Inventory";
      public const int AvailableQuantity = 14;
      public const int EditedAvailableQuantity = 114;
      public const double UnitPrice = 9.99;
      public const double EditedUnitPrice = 19.99;
      public const InventoryCategoryOptions Category = InventoryCategoryOptions.Flower;
      public const InventoryCategoryOptions EditedCategory = InventoryCategoryOptions.Card;

      public const int InventoriesCount = 14;

      public const string UtcNow = "25/3/2024 09:00:00 AM";
      public const string EditedUtcNow = "26/3/2024 09:00:00 AM";

      public const string Email = "purchase@email.com";
      public const string PurchaseOrderNumber = "purchase order number";

      public const string UserId = "E4C92608-6377-40F0-AE37-55D20525CCD9";
      public const string FirstName = "first";
      public const string LastName = "last";
      public const RoleTypeOptions RoleType = RoleTypeOptions.Demo;

      public const string RoleId = "3E971209-5B1C-4267-8029-49FA741F747E";

      public static string InventoryIdFromIndex(int index) => $"03950F56-120D-4A2F-8418-1978ED8B{index:0000}";

      public static string InventoryNameFromIndex(int index) => $"{InventoryName} {index}";
    }
  }
}
