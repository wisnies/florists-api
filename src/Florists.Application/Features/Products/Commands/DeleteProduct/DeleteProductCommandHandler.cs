using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Products;
using MediatR;

namespace Florists.Application.Features.Products.Commands.DeleteProduct
{
  public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ErrorOr<ProductResultDTO>>
  {
    private readonly IProductRepository _productRepository;
    private readonly IDateTimeService _dateTimeService;
    public DeleteProductCommandHandler(
      IProductRepository productRepository,
      IDateTimeService dateTimeService)
    {
      _productRepository = productRepository;
      _dateTimeService = dateTimeService;
    }

    public async Task<ErrorOr<ProductResultDTO>> Handle(
      DeleteProductCommand command,
      CancellationToken cancellationToken)
    {
      var productToDelete = await _productRepository.GetOneByIdAsync(command.ProductId);

      if (productToDelete is null)
      {
        return CustomErrors.Products.NotFound;
      }

      productToDelete.UpdatedAt = _dateTimeService.UtcNow;
      productToDelete.IsActive = false;

      var success = await _productRepository.SoftDeleteAsync(productToDelete);

      if (!success)
      {
        return CustomErrors.Database.SaveError;
      }

      return new ProductResultDTO(
        Messages.Database.DeleteSuccess,
        productToDelete);
    }
  }
}
