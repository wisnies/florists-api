namespace Florists.Core.Common.Messages
{
  public static partial class Messages
  {
    public static class GuidId
    {
      public const string IsRequired = "Id is required";
      public const string IsInvalid = "Id must be valid Guid value";
    }

    public static class InventoryName
    {
      public const string IsRequired = "Inventory name is required";
      public const string MinLengthIs = "Inventory name minimum length is ";
      public const string MaxLengthIs = "Inventory name maximum length is ";
    }

    public static class ProductName
    {
      public const string IsRequired = "Product name is required";
      public const string MinLengthIs = "Product name minimum length is ";
      public const string MaxLengthIs = "Product name maximum length is ";
    }

    public static class FirstName
    {
      public const string IsRequired = "First name is required";
      public const string MinLengthIs = "First name minimum length is ";
      public const string MaxLengthIs = "First name maximum length is ";
    }

    public static class LastName
    {
      public const string IsRequired = "Last name is required";
      public const string MinLengthIs = "Last name minimum length is ";
      public const string MaxLengthIs = "Last name maximum length is ";
    }

    public static class Sku
    {
      public const string IsRequired = "Sku is required";
      public const string MinLengthIs = "Sku minimum length is ";
      public const string MaxLengthIs = "Sku maximum length is ";
    }

    public static class OrderNumber
    {
      public const string IsRequired = "Order number is required";
      public const string MinLengthIs = "Order number minimum length is ";
      public const string MaxLengthIs = "Order number maximum length is ";
    }

    public static class Email
    {
      public const string IsRequired = "Email address is required";
      public const string IsInvalid = "This must be a valid email address";
      public const string IsDuplicate = "This email is already taken";
    }
  }
}
