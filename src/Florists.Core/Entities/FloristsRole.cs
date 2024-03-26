using Florists.Core.Enums;

namespace Florists.Core.Entities
{
  public class FloristsRole
  {
    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }
    public RoleTypeOptions RoleType { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
