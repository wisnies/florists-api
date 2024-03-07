using ErrorOr;

namespace Florists.Core.Common.CustomErrors
{
  public static partial class CustomErrors
  {
    public static class Products
    {
      public static Error AlreadyExists =>
        Error.Validation(
          code: "Validation.ProductName",
          description: Messages.Messages.Products.AlreadyExists);

      public static Error NotFound =>
        Error.NotFound(
          code: "NotFound.Product",
          description: Messages.Messages.Products.NotFound);
    }
  }
}
