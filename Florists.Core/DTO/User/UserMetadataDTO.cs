namespace Florists.Core.DTO.User
{
  public record UserMetadataDTO(
      Guid UserId,
      string FirstName,
      string LastName);
}
