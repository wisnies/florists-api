namespace Florists.Core.Entities
{
  public class FloristsUser
  {
    public Guid UserId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsPasswordChanged { get; set; } = false;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiration { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public FloristsRole? Role { get; set; }
  }
}
