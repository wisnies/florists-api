using Florists.Application.Features.Inventories.Commands.CreateInventory;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Inventories.Queries.TestUtils
{
  public static class CreateInventoryCommandUtils
  {
    public static CreateInventoryCommand CreateCommand()
    {
      return new CreateInventoryCommand(
        Constants.Inventories.InventoryName,
        Constants.Inventories.AvailableQuantity,
        Constants.Inventories.UnitPrice,
        Constants.Inventories.Category);
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
        CreatedAt = DateTime.Parse(Constants.Inventories.UtcNow)
      };
    }
  }
}
