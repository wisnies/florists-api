namespace Florists.Core.DTO.Auth
{
  public record AuthTokenDTO(
    string JwtToken,
    string RefreshToken);
}
