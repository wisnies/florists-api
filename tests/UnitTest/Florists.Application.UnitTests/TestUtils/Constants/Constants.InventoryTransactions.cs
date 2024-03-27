using Florists.Core.Enums;

namespace Florists.Application.UnitTests.TestUtils.Constants
{
  public static partial class Constants
  {
    public static class InventoryTransactions
    {
      public const string DateFrom = "25/3/2024 09:00:00 AM";
      public const string DateTo = "29/3/2024 09:00:00 AM";
      public const InventoryTransactionTypeOptions TransactionType = InventoryTransactionTypeOptions.PurchaseInventory;
      public const OrderOptions Order = OrderOptions.ASC;
      public const TransactionsOrderByOptions OrderBy = TransactionsOrderByOptions.transaction_value;

      public const string TransactionId = "1005FF20-1981-4AE8-AF35-D176C3A4C8AD";
      public const string InventoryId = "F904329D-B1D7-4A6E-B5C1-EC24127DB186";
      public const string UserId = "63DD9D14-455C-4DC8-945F-A119ABAE71F5";
      public const string PurchaseOrderNumber = "abc";
      public const string ProductionOrderNumber = "cde";
      public const int QuantityBefore = 10;
      public const int QuantityAfter = 12;
      public const double TransactionValue = 12.99;
      public const string CreatedAt = "25/3/2024 09:00:00 AM";

      public const string FirstName = "first";
      public const string LastName = "last";

      public const string InventoryName = "name";
      public const double UnitPrice = 9.99;
      public const InventoryCategoryOptions Category = InventoryCategoryOptions.Accessory;

      public const int TransactionsCount = 17;

      public static string PurchaseOrdernumberFromIndex(int i) => $"purchase {i}";
    }
  }
}
