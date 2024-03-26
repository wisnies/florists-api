using Florists.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Florists.Infrastructure.Policies
{
  public static class ForAdminPolicy
  {
    public const string Name = "ForAdmin";

    public static void AddForAdminPolicy(this AuthorizationPolicyBuilder policy)
    {
      policy.RequireClaim(ClaimTypes.Role, RoleTypeOptions.Admin.ToString());
    }
  }
}
