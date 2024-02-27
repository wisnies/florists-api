using Florists.Application.Interfaces.Services;
using Florists.Core.DTO.Auth;
using Florists.Core.Entities;
using Florists.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Florists.Infrastructure.Services
{
  public class TokenService : ITokenService
  {
    private readonly IDateTimeService _dateTimeService;
    private readonly TokenSettings _tokenSettings;
    private readonly TokenValidationParameters _validationParameters;

    public TokenService(
      IDateTimeService dateTimeService,
      IOptions<TokenSettings> tokenOptions)
    {
      _dateTimeService = dateTimeService;
      _tokenSettings = tokenOptions.Value;
      _validationParameters = new TokenValidationParameters()
      {
        ValidateAudience = true,
        ValidAudience = _tokenSettings.Audience,
        ValidateIssuer = true,
        ValidIssuer = _tokenSettings.Issuer,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_tokenSettings.Key)),
        ValidateLifetime = false,
      };
    }

    public UserTokensDTO GenerateToken(FloristsUser user)
    {
      var jwtTokenExpiration = _dateTimeService.UtcNow.AddMinutes(Convert.ToInt32(_tokenSettings.JwtExpiresInMinutes));

      var refreshTokenexpiration = _dateTimeService.UtcNow.AddMinutes(Convert.ToInt32(_tokenSettings.RefreshTokenExpiresInMinutes));

      var claims = new Claim[] { new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), new Claim(ClaimTypes.Name, user.FirstName + user.LastName), new Claim(ClaimTypes.Email, user.Email) };

      var securityKey = new SymmetricSecurityKey(
      Encoding.UTF8.GetBytes(_tokenSettings.Key));

      var signingCredentials = new SigningCredentials(
      securityKey,
      SecurityAlgorithms.HmacSha256);

      var tokenGenerator = new JwtSecurityToken(
      _tokenSettings.Issuer,
      _tokenSettings.Audience,
      claims: claims,
      expires: jwtTokenExpiration,
      signingCredentials: signingCredentials
      );

      var token = new JwtSecurityTokenHandler()
      .WriteToken(tokenGenerator);

      return new UserTokensDTO(
      JwtToken: token,
      JwtTokenExpiration: jwtTokenExpiration,
      RefreshToken: GenerateRefreshToken(),
      RefreshTokenExpiration: refreshTokenexpiration);
    }

    public ClaimsPrincipal? GetClaimsPrincipal(string token)
    {
      try
      {

        var principal = new JwtSecurityTokenHandler().ValidateToken(token, _validationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
          !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
        {
          return null;
        }

        return principal;
      }
      catch
      {
        return null;
      }
    }

    public bool IsValid(string token)
    {
      try
      {
        var principal = new JwtSecurityTokenHandler()
        .ValidateToken(
        token,
        _validationParameters,
        out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
        !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
        {
          return false;
        }

        return true;
      }
      catch
      {
        return false;
      }
    }

    private static string GenerateRefreshToken()
    {
      var bytes = new byte[64];

      var randomNumberGenerator = RandomNumberGenerator.Create();
      randomNumberGenerator.GetBytes(bytes);

      return Convert.ToBase64String(bytes);
    }
  }
}
