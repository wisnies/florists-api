using Florists.Application.Features.Inventories.Commands.PurchaseInventories;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Core.DTO.Flowers;

namespace Florists.Application.UnitTests.Inventories.Queries.TestUtils
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
            count + 1
          );
      }

      return inventoriesToPurchase;
    }
  }
}
