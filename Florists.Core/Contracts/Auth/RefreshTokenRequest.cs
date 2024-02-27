namespace Florists.Core.Contracts.Auth
{
  public record RefreshTokenRequest(
    string JwtToken,
    string RefreshToken);
}
