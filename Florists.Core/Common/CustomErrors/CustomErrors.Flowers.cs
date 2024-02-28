using ErrorOr;

namespace Florists.Core.Common.CustomErrors
{
  public static partial class CustomErrors
  {
    public static class Flowers
    {
      public static Error FlowerAlreadyExists =>
        Error.Validation(
          code: "Flower.FlowerAlreadyExists",
          description: Messages.Messages.Flowers.FlowerAlreadyExists);

      public static Error FlowerNotFound =>
        Error.NotFound(
          code: "Flower.FlowerNotFound",
          description: Messages.Messages.Flowers.FlowerNotFound);

      public static Error UnableToCreateFlower =>
        Error.Failure(
          code: "Flower.UnableToCreateFlower",
          description: Messages.Messages.Flowers.UnableToCreateFlower);

      public static Error UnableToEditFlower =>
        Error.Failure(
          code: "Flower.UnableToEditFlower",
          description: Messages.Messages.Flowers.UnableToEditFlower);

      public static Error UnableToFetchFlowers =>
        Error.Failure(
          code: "Flower.UnableToFetchFlowers",
          description: Messages.Messages.Flowers.UnableToFetchFlowers);

      public static Error UnableToPurchaseFlowers =>
        Error.Failure(
          code: "Flower.UnableToPurchaseFlowers",
          description: Messages.Messages.Flowers.UnableToPurchaseFlowers);
    }
  }
}
