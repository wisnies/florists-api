﻿using ErrorOr;
using Florists.Core.DTO.Common;
using Florists.Core.DTO.Products;
using MediatR;

namespace Florists.Application.Features.Products.Commands.SellProducts
{
  public record SellProductsCommand(
    string SaleOrderNumber,
    List<ProductToSellDTO> ProductsToSell,
    string Email) : IRequest<ErrorOr<MessageResultDTO>>;
}
