namespace Florists.Core.Common.Messages
{
  public static partial class Messages
  {
    public static class Flowers
    {
      public const string UnableToCreateFlower = "Unable to create flower, try again later";
      public const string UnableToEditFlower = "Unable to edit flower, try again later";
      public const string UnableToFetchFlowers = "Unable to fetch flowers, try again later";
      public const string UnableToPurchaseFlowers = "Unable to purchase flowers, try again later";

      public const string FlowerAlreadyExists = "Flower with that name already exists";
      public const string FlowerNotFound = "Flower not found";

      public const string FlowerIdIsRequired = "Flower id is required";

      public const string FlowerNameIsRequired = "Flower name is required";
      public const string FlowerNameMinLengthIs = "Flower name minimum length is ";
      public const string FlowerNameMaxLengthIs = "Flower name maximum length is ";

      public const string UnitPriceIsRequired = "Unit price name is required";
      public const string UnitPriceMustBeGreaterThanZero = "Unit price must be greater than zero";

      public const string AvailableQuantityIsRequired = "Available quantity is required";
      public const string AvailableQuantityMustBeGreaterThanZero = "Available quantity must be greater than zero";

      public const string CreateSuccess = "Flower created";
      public const string EditSuccess = "Flower updated";
      public const string FetchSuccess = "Data Fetched";
      public const string PurchaseSuccess = "Flowers purchased";
    }
  }
}
