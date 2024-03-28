using Florists.Application.Features.Products.Commands.CreateProduct;
using Florists.Application.Features.Products.Commands.DeleteProduct;
using Florists.Application.Features.Products.Commands.EditProduct;
using Florists.Application.Features.Products.Commands.ProduceProduct;
using Florists.Application.Features.Products.Commands.SellProducts;
using Florists.Application.Features.Products.Queries.GetProductById;
using Florists.Application.Features.Products.Queries.GetProductsByName;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Products;
using Florists.Core.DTO.ProductTransactions;
using Florists.Core.Entities;

namespace Florists.Application.UnitTests.TestUtils.Extenstions.Products
{
  public static partial class ProductExtensions
  {
    public static void ValidateCreatedFrom(this ProductsResultDTO result, GetProductsByNameQuery query, int offset)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.FetchSuccess));

        Assert.That(result.Count, Is.EqualTo(Constants.Constants.Products.ProductsCount));
        Assert.That(result.Products, Has.Count.LessThanOrEqualTo(query.PerPage));

        if (query.PerPage >= Constants.Constants.Products.ProductsCount)
        {
          Assert.That(result.Products, Has.Count.AtLeast(Constants.Constants.Products.ProductsCount));
        }

        for (int i = 0; i < result.Products.Count; i++)
        {
          var n = offset + i;
          Assert.That(
            result.Products[i].ProductName,
            Is.EqualTo(Constants.Constants.Products.ProductNameFromIndex(n)));
        }

      });
    }

    public static void ValidateCreatedFrom(this ProductResultDTO result, GetProductByIdQuery query, int inventoriesCount)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.FetchSuccess));

        Assert.That(result.Product, Is.Not.Null);
        Assert.That(result.Product, Is.TypeOf(typeof(Product)));
        Assert.That(result.Product.ProductInventories, Is.Not.Null);
        Assert.That(result.Product.ProductInventories, Has.Count.EqualTo(inventoriesCount));

        Assert.That(result.Product.ProductId, Is.TypeOf(typeof(Guid)));
        Assert.That(result.Product.ProductId, Is.EqualTo(Guid.Parse(Constants.Constants.Products.ProductId)));


        for (int i = 0; i < result.Product.ProductInventories!.Count; i++)
        {
          Assert.That(
             result.Product.ProductInventories[i].ProductId,
            Is.EqualTo(result.Product.ProductId));

          Assert.That(
             result.Product.ProductInventories[i].Inventory,
            Is.Not.Null);

          Assert.That(
             result.Product.ProductInventories[i].InventoryId,
            Is.EqualTo(result.Product.ProductInventories[i].Inventory!.InventoryId));

          Assert.That(
             result.Product.ProductInventories[i].Inventory!.InventoryName,
            Is.EqualTo(Constants.Constants.Products.InventorytNameFromIndex(i)));
        }
      });
    }

    public static void ValidateCreatedFrom(this ProductResultDTO result, CreateProductCommand command, int inventoriesCount)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.SaveSuccess));

        Assert.That(result.Product, Is.Not.Null);
        Assert.That(result.Product, Is.TypeOf(typeof(Product)));
        Assert.That(result.Product.ProductInventories, Is.Not.Null);
        Assert.That(result.Product.ProductInventories, Has.Count.EqualTo(inventoriesCount));

        Assert.That(result.Product.ProductId, Is.TypeOf(typeof(Guid)));


        for (int i = 0; i < result.Product.ProductInventories!.Count; i++)
        {
          Assert.That(
             result.Product.ProductInventories[i].ProductId,
            Is.EqualTo(result.Product.ProductId));

          Assert.That(
             result.Product.ProductInventories[i].InventoryId,
            Is.EqualTo(command.RequiredInventories[i].InventoryId));

          Assert.That(
             result.Product.ProductInventories[i].RequiredQuantity,
            Is.EqualTo(command.RequiredInventories[i].RequiredQuantity));

          Assert.That(
             result.Product.ProductInventories[i].Inventory,
            Is.Null);
        }
      });
    }

    public static void ValidateCreatedFrom(this ProductResultDTO result, DeleteProductCommand command)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.DeleteSuccess));

        Assert.That(result.Product, Is.Not.Null);
        Assert.That(result.Product, Is.TypeOf(typeof(Product)));

        Assert.That(result.Product.ProductId, Is.TypeOf(typeof(Guid)));
        Assert.That(result.Product.ProductId, Is.EqualTo(Guid.Parse(Constants.Constants.Products.ProductId)));
        Assert.That(result.Product.IsActive, Is.False);
        Assert.That(result.Product.UpdatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Products.EditedUtcNow)));

      });
    }

    public static void ValidateCreatedFrom(this ProductResultDTO result, EditProductCommand command, int inventoriesCount)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.UpdateSuccess));

        Assert.That(result.Product, Is.Not.Null);
        Assert.That(result.Product, Is.TypeOf(typeof(Product)));
        Assert.That(result.Product.ProductInventories, Is.Not.Null);
        Assert.That(result.Product.ProductInventories, Has.Count.EqualTo(inventoriesCount));

        Assert.That(result.Product.ProductId, Is.TypeOf(typeof(Guid)));
        Assert.That(result.Product.ProductId, Is.EqualTo(Guid.Parse(Constants.Constants.Products.ProductId)));
        Assert.That(result.Product.ProductName, Is.EqualTo(Constants.Constants.Products.EditedProductName));
        Assert.That(result.Product.Sku, Is.EqualTo(Constants.Constants.Products.EditedSku));
        Assert.That(result.Product.UnitPrice, Is.EqualTo(Constants.Constants.Products.EditedUnitPrice));
        Assert.That(result.Product.Category, Is.EqualTo(Constants.Constants.Products.EditedCategory));
        Assert.That(result.Product.UpdatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Products.EditedUtcNow)));


        for (int i = 0; i < result.Product.ProductInventories!.Count; i++)
        {
          Assert.That(
             result.Product.ProductInventories[i].ProductId,
            Is.EqualTo(result.Product.ProductId));

          Assert.That(
   result.Product.ProductInventories[i].InventoryId,
  Is.EqualTo(command.RequiredInventories[i].InventoryId));

          Assert.That(
             result.Product.ProductInventories[i].RequiredQuantity,
            Is.EqualTo(command.RequiredInventories[i].RequiredQuantity));

          Assert.That(
             result.Product.ProductInventories[i].CreatedAt,
            Is.EqualTo(result.Product.CreatedAt));

          Assert.That(
             result.Product.ProductInventories[i].Inventory,
            Is.Null);
        }
      });
    }

    public static void ValidateCreatedFrom(this ProductTransactionsResultDTO result, SellProductsCommand command, int productsCount)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.SaveSuccess));

        Assert.That(result.Transactions, Is.Not.Null);
        Assert.That(result.Transactions, Is.TypeOf(typeof(List<ProductTransaction>)));
        Assert.That(result.Transactions, Has.Count.EqualTo(productsCount));
        Assert.That(result.Count, Is.EqualTo(productsCount));

        for (int i = 0; i < result.Transactions.Count; i++)
        {
          var transaction = result.Transactions[i];
          var productToSell = command.ProductsToSell[i];
          Assert.That(transaction.CreatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Products.EditedUtcNow)));

          Assert.That(transaction.Product, Is.Not.Null);
          Assert.That(transaction.ProductId, Is.EqualTo(transaction.Product!.ProductId));
          Assert.That(transaction.Product.UpdatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Products.EditedUtcNow)));
          Assert.That(transaction.TransactionValue, Is.EqualTo(transaction.Product.UnitPrice * productToSell.QuantityToSell));
          Assert.That(transaction.QuantityAfter, Is.EqualTo(transaction.QuantityBefore - productToSell.QuantityToSell));
        }
      });
    }

    public static void ValidateCreatedFrom(this ProduceProductResultDTO result, ProduceProductCommand command, int inventoriesCount)
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Message, Is.EqualTo(Messages.Database.SaveSuccess));

        Assert.That(result.ProductTransaction, Is.Not.Null);
        Assert.That(result.ProductTransaction, Is.TypeOf(typeof(ProductTransaction)));
        Assert.That(result.ProductTransaction.ProductTransactionId, Is.TypeOf(typeof(Guid)));
        Assert.That(result.ProductTransaction.CreatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Products.EditedUtcNow)));
        Assert.That(result.ProductTransaction.ProductId, Is.EqualTo(Guid.Parse(Constants.Constants.Products.ProductId)));
        Assert.That(result.ProductTransaction.TransactionType, Is.EqualTo(Constants.Constants.Products.ProductProduceTransactionType));
        Assert.That(result.ProductTransaction.QuantityAfter, Is.EqualTo(result.ProductTransaction.QuantityBefore + command.QuantityToProduce));

        Assert.That(result.ProductTransaction.Product, Is.Not.Null);
        Assert.That(result.ProductTransaction.Product, Is.TypeOf(typeof(Product)));
        Assert.That(result.ProductTransaction.Product!.ProductId, Is.EqualTo(result.ProductTransaction.ProductId));
        Assert.That(result.ProductTransaction.Product!.ProductId, Is.EqualTo(command.ProductId));
        Assert.That(result.ProductTransaction.Product!.UpdatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Products.EditedUtcNow)));
        Assert.That(result.ProductTransaction.Product!.AvailableQuantity, Is.EqualTo(result.ProductTransaction.QuantityAfter));

        Assert.That(result.ProductTransaction.TransactionValue, Is.EqualTo(result.ProductTransaction.Product!.UnitPrice * command.QuantityToProduce));

        Assert.That(result.InventoryTransactions, Has.Count.EqualTo(inventoriesCount));


        for (int i = 0; i < result.InventoryTransactions.Count; i++)
        {
          var transaction = result.InventoryTransactions[i];
          Assert.That(transaction, Is.TypeOf(typeof(InventoryTransaction)));
          Assert.That(transaction.CreatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Products.EditedUtcNow)));
          Assert.That(transaction.TransactionType, Is.EqualTo(Constants.Constants.Products.InventoryTransactionType));
          Assert.That(transaction.InventoryTransactionId, Is.TypeOf(typeof(Guid)));

          Assert.That(transaction.Inventory, Is.Not.Null);
          Assert.That(transaction.Inventory, Is.TypeOf(typeof(Inventory)));
          Assert.That(transaction.Inventory!.InventoryId, Is.EqualTo(transaction.InventoryId));
          Assert.That(transaction.Inventory!.UpdatedAt, Is.EqualTo(DateTime.Parse(Constants.Constants.Products.EditedUtcNow)));
        }
      });
    }
  }
}
