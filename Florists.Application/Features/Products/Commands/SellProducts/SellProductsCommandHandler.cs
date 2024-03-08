using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Common;
using Florists.Core.Entities;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.Products.Commands.SellProducts
{
  public class SellProductsCommandHandler : IRequestHandler<SellProductsCommand, ErrorOr<MessageResultDTO>>
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

    public async Task<ErrorOr<MessageResultDTO>> Handle(
      SellProductsCommand command,
      CancellationToken cancellationToken)
    {
      var user = await _userRepository.GetOneByEmailAsync(command.Email);

      if (user is null)
      {
        return CustomErrors.Users.NotFound;
      }

      var transactions = new List<ProductTransaction>();

      var index = 0;
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
          return CustomErrors.Products.QuantityToSellUnavailable(index);
        }

        var transaction = new ProductTransaction
        {
          ProductTransactionId = Guid.NewGuid(),
          ProductId = product.ProductId,
          UserId = user.UserId,
          SaleOrderNumber = command.SaleOrderNumber,
          QuantityBefore = product.AvailableQuantity,
          QuantityAfter = product.AvailableQuantity - productToSell.QuantityToSell,
          TransactionValue = productToSell.QuantityToSell * product.UnitPrice,
          TransactionType = ProductTransactionOptions.SellProduct,
          CreatedAt = _dateTimeService.UtcNow,
          Product = product,
        };

        product.AvailableQuantity -= productToSell.QuantityToSell;
        product.UpdatedAt = _dateTimeService.UtcNow;

        transactions.Add(transaction);
        index++;
      }

      var success = await _productTransactionRepository.SellAsync(transactions);

      if (!success)
      {
        return CustomErrors.Database.SaveError;
      }

      return new MessageResultDTO(
        true,
        Messages.Database.SaveSuccess);
    }
  }
}
