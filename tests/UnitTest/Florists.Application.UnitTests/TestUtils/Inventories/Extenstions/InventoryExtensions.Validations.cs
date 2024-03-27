using Florists.Application.Features.Inventories.Commands.CreateInventory;
using Florists.Application.Features.Inventories.Commands.EditInventory;
using Florists.Application.Features.Inventories.Queries.GetInventoriesByName;
using Florists.Application.Features.Inventories.Queries.GetInventoryById;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Inventories;

namespace Florists.Application.UnitTests.TestUtils.Inventories.Extenstions
{
  public static partial class InventoryExtensions
  {
    public static void ValidateCreatedFrom(this InventoryResultDTO result, GetInventoryByIdQuery query)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Inventory.InventoryId, Is.EqualTo(Guid.Parse(Constants.Constants.Inventories.InventoryId)));
        Assert.That(result.Inventory.InventoryName, Is.EqualTo(Constants.Constants.Inventories.InventoryName));
        Assert.That(result.Inventory.UnitPrice, Is.EqualTo(Constants.Constants.Inventories.UnitPrice));
        Assert.That(result.Inventory.AvailableQuantity, Is.EqualTo(Constants.Constants.Inventories.AvailableQuantity));
        Assert.That(result.Inventory.Category, Is.EqualTo(Constants.Constants.Inventories.Category));
        Assert.That(result.Message, Is.EqualTo(Messages.Database.FetchSuccess));
      });
    }

    public static void ValidateCreatedFrom(this InventoriesResultDTO result, GetInventoriesByNameQuery query)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.FetchSuccess));
        Assert.That(result.Count, Is.EqualTo(Constants.Constants.Inventories.InventoriesCount));
        Assert.That(result.Inventories, Has.Count.EqualTo(query.PerPage));

        for (int i = 0; i < result.Inventories.Count; i++)
        {
          Assert.That(
            result.Inventories[i].InventoryName,
            Is.EqualTo(Constants.Constants.Inventories.InventoryNameFromIndex(i)));
        }

      });
    }

    public static void ValidateCreatedFrom(this InventoryResultDTO result, CreateInventoryCommand command)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Inventory.InventoryId, Is.TypeOf(typeof(Guid)));
        Assert.That(result.Inventory.InventoryName, Is.EqualTo(Constants.Constants.Inventories.InventoryName));
        Assert.That(result.Inventory.UnitPrice, Is.EqualTo(Constants.Constants.Inventories.UnitPrice));
        Assert.That(result.Inventory.AvailableQuantity, Is.EqualTo(Constants.Constants.Inventories.AvailableQuantity));
        Assert.That(result.Inventory.Category, Is.EqualTo(Constants.Constants.Inventories.Category));
        Assert.That(result.Inventory.CreatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Inventories.UtcNow)));
        Assert.That(result.Message, Is.EqualTo(Messages.Database.SaveSuccess));
      });
    }

    public static void ValidateCreatedFrom(this InventoryResultDTO result, EditInventoryCommand command)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Inventory.InventoryId, Is.EqualTo(Guid.Parse(Constants.Constants.Inventories.InventoryId)));
        Assert.That(result.Inventory.InventoryName, Is.EqualTo(Constants.Constants.Inventories.EditedInventoryName));
        Assert.That(result.Inventory.UnitPrice, Is.EqualTo(Constants.Constants.Inventories.EditedUnitPrice));
        Assert.That(result.Inventory.AvailableQuantity, Is.EqualTo(Constants.Constants.Inventories.EditedAvailableQuantity));
        Assert.That(result.Inventory.Category, Is.EqualTo(Constants.Constants.Inventories.EditedCategory));
        Assert.That(result.Inventory.UpdatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Inventories.EditedUtcNow)));
        Assert.That(result.Message, Is.EqualTo(Messages.Database.SaveSuccess));
      });
    }
  }
}
