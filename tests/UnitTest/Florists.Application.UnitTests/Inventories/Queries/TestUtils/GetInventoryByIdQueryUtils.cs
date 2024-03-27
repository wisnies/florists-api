using Florists.Application.Features.Inventories.Queries.GetInventoryById;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Inventories.Queries.TestUtils
{
  public class GetInventoryByIdQueryUtils
  {
    public static GetInventoryByIdQuery CreateQuery()
    {
      return new GetInventoryByIdQuery(
        Guid.Parse(Constants.Inventories.InventoryId));
    }

    public static Inventory CreateInventory()
    {
      return new Inventory
      {
        InventoryId = Guid.Parse(Constants.Inventories.InventoryId),
        InventoryName = Constants.Inventories.InventoryName,
        AvailableQuantity = Constants.Inventories.AvailableQuantity,
        UnitPrice = Constants.Inventories.UnitPrice,
        Category = Constants.Inventories.Category,
      };
    }
  }
}
