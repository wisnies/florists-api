namespace Florists.Core.DTO.Auth
{
  public record UserTokensDTO(
    string JwtToken,
    DateTime JwtTokenExpiration,
    string RefreshToken,
    DateTime RefreshTokenExpiration);
}
