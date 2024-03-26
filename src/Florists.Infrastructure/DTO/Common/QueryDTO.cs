namespace Florists.Infrastructure.DTO.Common
{
  public record QueryDTO(
    string Sql,
    object Parameters);
}
