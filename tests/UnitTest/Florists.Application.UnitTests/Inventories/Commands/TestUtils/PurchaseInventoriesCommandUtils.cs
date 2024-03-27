using Florists.Application.Features.Inventories.Commands.PurchaseInventories;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.DTO.Flowers;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.Inventories.Commands.TestUtils
{
  public static class PurchaseInventoriesCommandUtils
  {
    public static PurchaseInventoriesCommand CreateCommand(int inventoriesCount = 10)
    {
      return new PurchaseInventoriesCommand(
        Constants.Inventories.Email,
        Constants.Inventories.PurchaseOrderNumber,
        CreateInventoriesToPurchase(inventoriesCount));
    }

    public static List<InventoryToPurchaseDTO> CreateInventoriesToPurchase(int count)
    {
      var inventoriesToPurchase = new List<InventoryToPurchaseDTO>(count);

      for (int i = 0; i < count; i++)
      {
        var dto = new InventoryToPurchaseDTO(
            Guid.Parse(Constants.Inventories.InventoryIdFromIndex(i)),
            count + i
          );

        inventoriesToPurchase.Add(dto);
      }

      return inventoriesToPurchase;
    }

    public static FloristsUser CreateUser()
    {
      return new FloristsUser
      {
        UserId = Guid.Parse(Constants.Inventories.UserId),
        Email = Constants.Inventories.Email,
        FirstName = Constants.Inventories.FirstName,
        LastName = Constants.Inventories.LastName,
        CreatedAt = DateTime.Parse(Constants.Inventories.UtcNow),
        Role = new FloristsRole
        {
          RoleId = Guid.Parse(Constants.Inventories.RoleId),
          UserId = Guid.Parse(Constants.Inventories.UserId),
          RoleType = Constants.Inventories.RoleType,
          CreatedAt = DateTime.Parse(Constants.Inventories.UtcNow),
        }
      };
    }

    public static Inventory CreateInventory()
    {
      return new Inventory
      {
        InventoryId = Guid.Parse(Constants.Inventories.InventoryId),
        InventoryName = Constants.Inventories.InventoryName,
        AvailableQuantity = 0,
        UnitPrice = Constants.Inventories.UnitPrice,
        Category = Constants.Inventories.Category,
        CreatedAt = DateTime.Parse(Constants.Inventories.UtcNow),
        UpdatedAt = DateTime.Parse(Constants.Inventories.UtcNow),
      };
    }
  }
}
