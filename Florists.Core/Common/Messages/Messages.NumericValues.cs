namespace Florists.Core.Common.Messages
{
  public static partial class Messages
  {
    public static class UnitPrice
    {
      public const string IsRequired = "Unit price is required";
      public const string MustBeGreaterThanZero = "Unit price must be greater than zero";
    }

    public static class Quantity
    {
      public const string IsRequired = "Quantity is required";
      public const string MustBeGreaterThanZero = "Quantity must be greater than zero";
    }

    public static class Collections
    {
      public const string LengthMustBeGreaterThanZero = "Select at least one item";
    }
  }
}
