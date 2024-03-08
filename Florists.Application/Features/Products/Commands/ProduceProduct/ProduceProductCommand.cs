using ErrorOr;
using Florists.Core.DTO.Common;
using MediatR;

namespace Florists.Application.Features.Products.Commands.ProduceProduct
{
  public record ProduceProductCommand(
    Guid ProductId,
    int QuantityToProduce,
    string ProductionOrderNumber,
    string Email) : IRequest<ErrorOr<MessageResultDTO>>;
}
