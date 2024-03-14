using Florists.Core.Enums;

namespace Florists.Core.DTO.Products
{
  public record ProductTransactionDataDTO(
    Guid ProductId,
    string ProductName,
    double UnitPrice,
    ProductCategoryOptions Category);
}
