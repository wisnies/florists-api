using Florists.Core.Enums;

namespace Florists.Application.UnitTests.TestUtils.Constants
{
  public static partial class Constants
  {
    public static class Products
    {
      public const string ProductId = "5F593111-D8FA-4BE4-918E-36EB448C8F2E";
      public const string ProductName = "product name";
      public const string EditedProductName = "product name";
      public const string Sku = "sku123sku";
      public const string EditedSku = "sku123sku";
      public const int AvailableQuantity = 99999;
      public const double UnitPrice = 17.99;
      public const double EditedUnitPrice = 17.99;
      public const ProductCategoryOptions Category = ProductCategoryOptions.Bouquet;
      public const ProductCategoryOptions EditedCategory = ProductCategoryOptions.Bouquet;

      public const string UtcNow = "25/3/2024 09:00:00 AM";
      public const string EditedUtcNow = "26/3/2024 09:00:00 AM";

      public const int ProductsCount = 19;

      public const string InventoryId = "807C50F2-92A3-4513-B6BD-A67D2D4F91EE";
      public const string InventoryName = "Inventory Test";
      public const int InventoryAvailableQuantity = 3;
      public const double InventoryUnitPrice = 5.25;
      public const InventoryCategoryOptions InventoryCategory = InventoryCategoryOptions.Flower;

      public const string UserId = "E4C92608-6377-40F0-AE37-55D20525CCD9";
      public const string Email = "product@email.com";
      public const string FirstName = "first";
      public const string LastName = "last";

      public const string RoleId = "3E971209-5B1C-4267-8029-49FA741F747E";
      public const RoleTypeOptions RoleType = RoleTypeOptions.Demo;

      public const string SaleOrderNumber = "sale order number";
      public const string ProductionOrderNumber = "production order number";

      public const ProductTransactionTypeOptions ProductProduceTransactionType = ProductTransactionTypeOptions.ProduceProduct;
      public const InventoryTransactionTypeOptions InventoryTransactionType = InventoryTransactionTypeOptions.ProduceProduct;

      public static string ProductNameFromIndex(int index) => $"{ProductName} {index}";
      public static string ProductIdFromIndex(int index) => $"03950F56-120D-4A2F-8418-1978ED8B{index:0000}";
      public static string InventoryIdFromIndex(int index) => $"03950F56-120D-4A2F-8418-1978ED8B{index:0000}";

      public static string InventorytNameFromIndex(int index) => $"{InventoryName} {index}";
    }
  }
}
