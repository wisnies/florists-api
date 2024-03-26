using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Products;
using MediatR;

namespace Florists.Application.Features.Products.Queries.GetProductById
{
  public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ErrorOr<ProductResultDTO>>
  {
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
      _productRepository = productRepository;
    }

    public async Task<ErrorOr<ProductResultDTO>> Handle(
      GetProductByIdQuery query,
      CancellationToken cancellationToken)
    {
      var product = await _productRepository.GetOneByIdAsync(query.ProductId, true);

      if (product is null)
      {
        return CustomErrors.Products.NotFound;
      }

      return new ProductResultDTO(
        Messages.Database.FetchSuccess,
        product);
    }
  }
}
