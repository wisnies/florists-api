using Florists.Application.Features.Inventories.Queries.GetInventoriesByName;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Inventories.Queries.TestUtils
{
  public static class GetInventoriesByNameUtils
  {
    public static GetInventoriesByNameQuery CreateQuery(int perPage = 10, int page = 1)
    {
      return new GetInventoriesByNameQuery(
        Constants.Inventories.InventoryName,
        page,
        perPage);
    }

    public static List<Inventory> CreateInventories(int count = 10)
    {
      var inventories = new List<Inventory>(count);

      for (int i = 0; i < count; i++)
      {
        var inventory = new Inventory
        {
          InventoryId = Guid.NewGuid(),
          InventoryName = Constants.Inventories.InventoryNameFromIndex(i),
          AvailableQuantity = Constants.Inventories.AvailableQuantity,
          UnitPrice = Constants.Inventories.UnitPrice,
          Category = Constants.Inventories.Category,
        };
        inventories.Add(inventory);
      }

      return inventories;
    }
  }
}
