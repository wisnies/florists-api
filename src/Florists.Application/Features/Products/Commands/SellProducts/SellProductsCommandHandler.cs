using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.ProductTransactions;
using Florists.Core.Entities;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.Products.Commands.SellProducts
{
  public class SellProductsCommandHandler : IRequestHandler<SellProductsCommand, ErrorOr<ProductTransactionsResultDTO>>
  {
    private readonly IProductRepository _productRepository;
    private readonly IProductTransactionRepository _productTransactionRepository;
    private readonly IDateTimeService _dateTimeService;
    private readonly IUserRepository _userRepository;

    public SellProductsCommandHandler(
      IProductRepository productRepository,
      IProductTransactionRepository productTransactionRepository,
      IDateTimeService dateTimeService,
      IUserRepository userRepository)
    {
      _productRepository = productRepository;
      _productTransactionRepository = productTransactionRepository;
      _dateTimeService = dateTimeService;
      _userRepository = userRepository;
    }

    public async Task<ErrorOr<ProductTransactionsResultDTO>> Handle(
      SellProductsCommand command,
      CancellationToken cancellationToken)
    {
      var user = await _userRepository.GetOneByEmailAsync(command.Email);

      if (user is null)
      {
        return CustomErrors.Users.NotFound;
      }

      var transactions = new List<ProductTransaction>();

      foreach (var productToSell in command.ProductsToSell)
      {
        var product = await _productRepository
          .GetOneByIdAsync(productToSell.ProductId);

        if (product is null)
        {
          return CustomErrors.Products.NotFound;
        }

        if (product.AvailableQuantity < productToSell.QuantityToSell)
        {
          return CustomErrors.Products.QuantityToSellUnavailable(product.ProductName);
        }

        var quantityBefore = product.AvailableQuantity;
        var quantityAfter = product.AvailableQuantity - productToSell.QuantityToSell;

        product.AvailableQuantity = quantityAfter;
        product.UpdatedAt = _dateTimeService.UtcNow;

        var transaction = new ProductTransaction
        {
          ProductTransactionId = Guid.NewGuid(),
          ProductId = product.ProductId,
          UserId = user.UserId,
          SaleOrderNumber = command.SaleOrderNumber,
          QuantityBefore = quantityBefore,
          QuantityAfter = quantityAfter,
          TransactionValue = productToSell.QuantityToSell * product.UnitPrice,
          TransactionType = ProductTransactionTypeOptions.SellProduct,
          CreatedAt = _dateTimeService.UtcNow,
          Product = product,
        };

        transactions.Add(transaction);
      }

      var success = await _productTransactionRepository.SellAsync(transactions);

      if (!success)
      {
        return CustomErrors.Database.SaveError;
      }

      return new ProductTransactionsResultDTO(
        Messages.Database.SaveSuccess,
        transactions.Count,
        transactions);
    }
  }
}
