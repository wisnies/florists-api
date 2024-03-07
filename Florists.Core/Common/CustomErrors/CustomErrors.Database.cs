using ErrorOr;

namespace Florists.Core.Common.CustomErrors
{
  public static partial class CustomErrors
  {
    public static class Database
    {
      public static Error SaveError =>
        Error.Unexpected(
          code: "Unexpected.Save",
          description: Messages.Messages.Database.SaveError);

      public static Error FetchError =>
        Error.Unexpected(
          code: "Unexpected.Fetch",
          description: Messages.Messages.Database.FetchError);
    }
  }
}
