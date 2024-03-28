using ErrorOr;
using Florists.Core.DTO.Products;
using Florists.Core.DTO.ProductTransactions;
using MediatR;

namespace Florists.Application.Features.Products.Commands.SellProducts
{
  public record SellProductsCommand(
    string SaleOrderNumber,
    List<ProductToSellDTO> ProductsToSell,
    string Email) : IRequest<ErrorOr<ProductTransactionsResultDTO>>;
}
