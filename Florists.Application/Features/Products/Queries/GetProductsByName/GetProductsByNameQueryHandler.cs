using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Products;
using MediatR;

namespace Florists.Application.Features.Products.Queries.GetProductsByName
{
  public class GetProductsByNameQueryHandler : IRequestHandler<GetProductsByNameQuery, ErrorOr<ProductsResultDTO>>
  {
    private readonly IProductRepository _productRepository;

    public GetProductsByNameQueryHandler(IProductRepository productRepository)
    {
      _productRepository = productRepository;
    }

    public async Task<ErrorOr<ProductsResultDTO>> Handle(
      GetProductsByNameQuery query,
      CancellationToken cancellationToken)
    {
      var offset = query.PerPage * (query.Page - 1);

      var products = await _productRepository.GetManyByNameAsync(
        query.ProductName,
        offset,
        query.PerPage);


      if (products is null)
      {
        return CustomErrors.Database.FetchError;
      }

      var count = await _productRepository.CountByNameAsync(query.ProductName);

      return new ProductsResultDTO(
        Messages.Database.FetchSuccess,
        count,
        products);
    }
  }
}
