using Florists.Core.Enums;

namespace Florists.Application.UnitTests.TestUtils.Constants
{
  public static partial class Constants
  {
    public static class Auth
    {
      public const string UserId = "03950F56-120D-4A2F-8418-1978ED8BEE15";
      public const bool IsActive = true;
      public const bool IsPasswordChanged = true;
      public const string Email = "janusz@example.com";
      public const string FirstName = "janusz@example.com";
      public const string LastName = "janusz@example.com";
      public const string Password = "P@s5w0rD";
      public const string PasswordHash = "P@s5w0rD";
      public const string NewPassword = "P@s5w0rDabc";

      public const string RoleId = "0CFA2ACC-3C70-455E-90ED-5B2BA83DB4B9";
      public const RoleTypeOptions RoleType = RoleTypeOptions.Demo;

      public const string TokenJti = "47021ED3-1D3A-409E-8F7E-1F8442D4E48E";
      public const string JwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlN2RiM2VlYS1hNjJhLTRhYzEtOTcwNy03ZWUyZGE2MjM1N2EiLCJqdGkiOiJlMjZhZTI0Ni05NDc1LTQ5NDYtODNmYS00MDE5MjMwNjM4YmYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibWF0ZXVzejF3acWbbmlld3NraSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Im1hdGV1c3pAZXhhbXBsZS5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTcxMDQxNzYyOSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzIwOSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyMDkifQ.g6kpk3VTLC9oMIizKAagH9Wq0HVHZEMakc4mywCTjbA";
      public const string RefreshToken = "H8sgeoTUJZsvGlwUbtaLetSw/OWHXfR3SWqK2a1GCZDwCgdar+2QMQq4zIwP/8u7bQabFpmO/e0u6suyvKDERw==";
      public const string JwtTokenExpiration = "25/3/2024 09:00:00 AM";
      public const string RefreshTokenExpiration = "25/3/2024 09:00:00 AM";
      public const string ValidUtcNow = "25/3/2024 09:00:00 AM";
      public const string InvalidUtcNow = "25/3/2099 09:00:00 AM";

      public const string ValidRefreshTokenExpiration = "25/3/2099 09:00:00 AM";
      public const string ExpiredRefreshTokenExpiration = "25/3/2023 09:00:00 AM";
    }
  }
}
