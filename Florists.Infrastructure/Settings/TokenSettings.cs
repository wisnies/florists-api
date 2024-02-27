namespace Florists.Infrastructure.Settings
{
  public class TokenSettings
  {
    public const string SectionName = "TokenSettings";
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int JwtExpiresInMinutes { get; set; }
    public int RefreshTokenExpiresInMinutes { get; set; }
  }
}
