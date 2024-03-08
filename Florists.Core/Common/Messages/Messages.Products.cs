namespace Florists.Core.Common.Messages
{
  public static partial class Messages
  {
    public static class Products
    {
      public const string NotFound = "Selected product not found";
      public const string AlreadyExists = "Product with that name already exists";

      public const string QuantityUnavailable = "Selected quantity is grater than available quantity";

      public const string CategoryIsRequired = "Product category is required";
      public const string CategoryIsInvalid = "Product category is invalid";
    }
  }
}
