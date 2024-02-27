using Florists.Core.Enums;

namespace Florists.Infrastructure.Records
{
  public class UserRecordWithRole
  {
    public Guid user_id { get; set; }
    public Guid role_id { get; set; }
    public RoleTypeOptions role_type { get; set; }
    public bool is_active { get; set; }
    public bool is_password_changed { get; set; }
    public string password_hash { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string first_name { get; set; } = string.Empty;
    public string last_name { get; set; } = string.Empty;
    public string refresh_token { get; set; } = string.Empty;
    public DateTime refresh_token_expiration { get; set; }
    public DateTime user_created_at { get; set; }
    public DateTime user_updated_at { get; set; }
    public DateTime role_created_at { get; set; }
    public DateTime role_updated_at { get; set; }
  }
}
