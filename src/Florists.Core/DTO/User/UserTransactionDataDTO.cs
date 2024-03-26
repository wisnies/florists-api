namespace Florists.Core.DTO.User
{
  public record UserTransactionDataDTO(
      Guid UserId,
      string FirstName,
      string LastName);
}
