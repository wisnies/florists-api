using Florists.Application.Interfaces.Services;
using Florists.Core.DTO.Auth;
using Florists.Core.Entities;
using Florists.Infrastructure.Services;
using Florists.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Florists.Test.Unit.Services
{
  public class TokenServiceTests
  {
    private ITokenService _cut;

    private Mock<IDateTimeService> _dateTimeServiceMock;
    private Mock<IOptions<TokenSettings>> _tokenOptionsMock;
    private FloristsUser _user;
    private TokenSettings _tokenSettings;

    [SetUp]
    public void Setup()
    {
      _tokenSettings = new TokenSettings
      {
        Key = "LKNB98haH98yhiubDDws78nKJB567kKHVjhv7678576f7gvun",
        Issuer = "https://localhost:7209",
        Audience = "https://localhost:7209",
        JwtExpiresInMinutes = 60,
        RefreshTokenExpiresInMinutes = 60,
      };
      _user = new FloristsUser
      {
        UserId = Guid.Parse("348CA716-2866-4C52-8CA8-9546CF1F64E7"),
        FirstName = "Janusz",
        LastName = "Tracz",
        Email = "janusz@example.com",
        Role = new FloristsRole
        {
          RoleId = Guid.Parse("348CA716-2866-4C52-8CA8-9546CF1F64E7"),
          RoleType = Core.Enums.RoleTypeOptions.Demo
        }
      };

      _dateTimeServiceMock = new Mock<IDateTimeService>();
      _tokenOptionsMock = new Mock<IOptions<TokenSettings>>();

      _dateTimeServiceMock
        .Setup(x => x.UtcNow)
        .Returns(DateTime.Parse("25/3/2024 09:00:00 AM"));

      _tokenOptionsMock.Setup(x => x.Value).Returns(_tokenSettings);

      _cut = new TokenService(
        _dateTimeServiceMock.Object,
        _tokenOptionsMock.Object);
    }

    [Test]
    public void GenerateToken_ReturnsUserTokensDTO_WhenValidUserProdived()
    {
      var actual = _cut.GenerateToken(_user);
      Assert.That(actual, Is.TypeOf(typeof(UserTokensDTO)));
    }

    [Test]
    public void GenerateToken_ReturnsExpectedJwtTokenExpiration_WhenValidUserProdived()
    {
      var jwtTokenExpiration = DateTime.Parse("25/3/2024 10:00:00 AM");

      var actual = _cut.GenerateToken(_user);
      Assert.That(actual.JwtTokenExpiration, Is.EqualTo(jwtTokenExpiration));
    }

    [Test]
    public void GenerateToken_ReturnsExpectedRefreshTokenExpiration_WhenValidUserProdived()
    {
      var refreshTokenExpiration = DateTime.Parse("25/3/2024 10:00:00 AM");

      var actual = _cut.GenerateToken(_user);
      Assert.That(actual.RefreshTokenExpiration, Is.EqualTo(refreshTokenExpiration));
    }

    [Test]
    public void GenerateToken_ReturnsStringJwtToken_WhenValidUserProdived()
    {
      var actual = _cut.GenerateToken(_user);
      Assert.That(actual.JwtToken, Is.TypeOf(typeof(string)));
    }

    [Test]
    public void GenerateToken_ReturnsStringRefreshToken_WhenValidUserProdived()
    {
      var actual = _cut.GenerateToken(_user);
      Assert.That(actual.RefreshToken, Is.TypeOf(typeof(string)));
    }

    [Test]
    public void GenerateToken_ReturnsValidJwtToken_WhenValidUserProdived()
    {
      var userTokensDTO = _cut.GenerateToken(_user);
      var actual = new JwtSecurityTokenHandler().ReadJwtToken(userTokensDTO.JwtToken);

      Assert.That(actual, Is.TypeOf(typeof(JwtSecurityToken)));
    }

    [TestCase("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlN2RiM2VlYS1hNjJhLTRhYzEtOTcwNy03ZWUyZGE2MjM1N2EiLCJqdGkiOiJlMjZhZTI0Ni05NDc1LTQ5NDYtODNmYS00MDE5MjMwNjM4YmYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibWF0ZXVzejF3acWbbmlld3NraSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Im1hdGV1c3pAZXhhbXBsZS5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTcxMDQxNzYyOSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzIwOSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyMDkifQ.g6kpk3VTLC9oMIizKAagH9Wq0HVHZEMakc4mywCTjbA")]
    public void IsValid_ReturnsTrue_WhenValidTokenProvided(string token)
    {
      Assert.That(_cut.IsValid(token), Is.True);
    }

    [TestCase("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9eyJzdWIiOiJlN2RiM2VlYS1hNjJhLTRhYzEtOTcwNy03ZWUyZGE2MjM1N2EiLCJqdGkiOiJlMjZhZTI0Ni05NDc1LTQ5NDYtODNmYS00MDE5MjMwNjM4YmYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibWF0ZXVzejF3acWbbmlld3NraSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Im1hdGV1c3pAZXhhbXBsZS5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTcxMDQxNzYyOSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzIwOSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyMDkifQ.g6kpk3VTLC9oMIizKAagH9Wq0HVHZEMakc4mywCTjbA")]
    [TestCase("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlN2RiM2VlYS1hNjJhLTRhYzEtOTcwNy03ZWUyZGE2MjM1N2EiLCJqdGkiOiJlMjZhZTI0Ni05NDc1LTQ5NDYtODNmYS00MDE5MjMwNjM4YmYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibWF0ZXVzejF3acWbbmlld3NraSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Im1hdGV1c3pAZXhhbXBsZS5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTcxMDQxNzYyOSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzIwOSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyMDkifQg6kpk3VTLC9oMIizKAagH9Wq0HVHZEMakc4mywCTjbA")]
    [TestCase("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9eyJzdWIiOiJlN2RiM2VlYS1hNjJhLTRhYzEtOTcwNy03ZWUyZGE2MjM1N2EiLCJqdGkiOiJlMjZhZTI0Ni05NDc1LTQ5NDYtODNmYS00MDE5MjMwNjM4YmYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibWF0ZXVzejF3acWbbmlld3NraSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Im1hdGV1c3pAZXhhbXBsZS5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTcxMDQxNzYyOSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzIwOSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyMDkifQg6kpk3VTLC9oMIizKAagH9Wq0HVHZEMakc4mywCTjbA")]
    public void IsValid_ReturnsFalse_WhenInvalidTokenProvided(string token)
    {
      Assert.That(_cut.IsValid(token), Is.False);
    }

    [TestCase("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlN2RiM2VlYS1hNjJhLTRhYzEtOTcwNy03ZWUyZGE2MjM1N2EiLCJqdGkiOiJlMjZhZTI0Ni05NDc1LTQ5NDYtODNmYS00MDE5MjMwNjM4YmYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibWF0ZXVzejF3acWbbmlld3NraSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Im1hdGV1c3pAZXhhbXBsZS5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTcxMDQxNzYyOSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzIwOSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyMDkifQ.g6kpk3VTLC9oMIizKAagH9Wq0HVHZEMakc4mywCTjbA")]
    public void GetClaimsPrincipal_ReturnsClaimsPrincipal_WhenValidTokenProvided(string token)
    {
      var actual = _cut.GetClaimsPrincipal(token);

      Assert.That(actual, Is.TypeOf(typeof(ClaimsPrincipal)));
    }

    [TestCase("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9eyJzdWIiOiJlN2RiM2VlYS1hNjJhLTRhYzEtOTcwNy03ZWUyZGE2MjM1N2EiLCJqdGkiOiJlMjZhZTI0Ni05NDc1LTQ5NDYtODNmYS00MDE5MjMwNjM4YmYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibWF0ZXVzejF3acWbbmlld3NraSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Im1hdGV1c3pAZXhhbXBsZS5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTcxMDQxNzYyOSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzIwOSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyMDkifQ.g6kpk3VTLC9oMIizKAagH9Wq0HVHZEMakc4mywCTjbA")]
    [TestCase("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlN2RiM2VlYS1hNjJhLTRhYzEtOTcwNy03ZWUyZGE2MjM1N2EiLCJqdGkiOiJlMjZhZTI0Ni05NDc1LTQ5NDYtODNmYS00MDE5MjMwNjM4YmYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibWF0ZXVzejF3acWbbmlld3NraSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Im1hdGV1c3pAZXhhbXBsZS5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTcxMDQxNzYyOSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzIwOSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyMDkifQg6kpk3VTLC9oMIizKAagH9Wq0HVHZEMakc4mywCTjbA")]
    [TestCase("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9eyJzdWIiOiJlN2RiM2VlYS1hNjJhLTRhYzEtOTcwNy03ZWUyZGE2MjM1N2EiLCJqdGkiOiJlMjZhZTI0Ni05NDc1LTQ5NDYtODNmYS00MDE5MjMwNjM4YmYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibWF0ZXVzejF3acWbbmlld3NraSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Im1hdGV1c3pAZXhhbXBsZS5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTcxMDQxNzYyOSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzIwOSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyMDkifQg6kpk3VTLC9oMIizKAagH9Wq0HVHZEMakc4mywCTjbA")]
    public void GetClaimsPrincipal_ReturnsNull_WhenInvalidTokenProvided(string token)
    {
      Assert.That(_cut.GetClaimsPrincipal(token), Is.Null);
    }

    [Test]
    public void GetClaimsPrincipal_ReturnsMatchingClaimsPrincipal_WhenValidTokenProvided()
    {
      var userTokensDTO = _cut.GenerateToken(_user);
      var actual = _cut.GetClaimsPrincipal(userTokensDTO.JwtToken);

      Assert.Multiple(() =>
      {
        //Assert.That(actual!.FindFirstValue(JwtRegisteredClaimNames.Sub),
        //  Is.EqualTo(_user.UserId.ToString()));
        Assert.That(actual!.FindFirstValue(JwtRegisteredClaimNames.Jti),
          Has.Length.EqualTo(36));
        Assert.That(actual!.FindFirstValue(ClaimTypes.Email),
          Is.EqualTo(_user.Email));
        Assert.That(actual!.FindFirstValue(ClaimTypes.Name),
          Is.EqualTo(_user.FirstName + _user.LastName));
        Assert.That(actual!.FindFirstValue(ClaimTypes.Role),
          Is.EqualTo(_user.Role!.RoleType.ToString()));
      });

    }
  }
}
