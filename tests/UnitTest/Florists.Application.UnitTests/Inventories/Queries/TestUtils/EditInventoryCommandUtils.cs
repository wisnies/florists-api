using Florists.Application.Features.Inventories.Commands.EditInventory;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Inventories.Queries.TestUtils
{
  public static class EditInventoryCommandUtils
  {
    public static EditInventoryCommand CreateCommand()
    {
      return new EditInventoryCommand(
        Guid.Parse(Constants.Inventories.InventoryId),
        Constants.Inventories.EditedInventoryName,
        Constants.Inventories.EditedAvailableQuantity,
        Constants.Inventories.EditedUnitPrice,
        Constants.Inventories.EditedCategory
        );
    }

    public static Inventory CreateInventory(bool isSameName = false)
    {
      return new Inventory
      {
        InventoryId = isSameName ? Guid.Parse(Constants.Inventories.OtherInventoryId) : Guid.Parse(Constants.Inventories.InventoryId),
        InventoryName = Constants.Inventories.InventoryName,
        AvailableQuantity = Constants.Inventories.AvailableQuantity,
        UnitPrice = Constants.Inventories.UnitPrice,
        Category = Constants.Inventories.Category,
        CreatedAt = DateTime.Parse(Constants.Inventories.UtcNow),
        UpdatedAt = DateTime.Parse(Constants.Inventories.UtcNow),
      };
    }
  }
}
