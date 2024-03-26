using ErrorOr;

namespace Florists.Core.Common.CustomErrors
{
  public static partial class CustomErrors
  {
    public static class Inventories
    {
      public static Error AlreadyExists =>
        Error.Validation(
          code: "Validation.InventoryName",
          description: Messages.Messages.Inventories.AlreadyExists);

      public static Error NotFound =>
        Error.NotFound(
          code: "NotFound.InventoryNotFound",
          description: Messages.Messages.Inventories.NotFound);

      public static Error InsufficientQuantity(string inventoryName)
      {
        return Error.Validation(
          code: "Validation.QuantityToProduce",
          description: Messages.Messages.Inventories.InsufficientQuantity + ": " + inventoryName);
      }
    }
  }
}
