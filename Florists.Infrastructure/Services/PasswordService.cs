using Florists.Application.Interfaces.Services;
using System.Text.RegularExpressions;

namespace Florists.Infrastructure.Services
{
  public class PasswordService : IPasswordService
  {
    public async Task<string> GenerateHashAsync(string password)
    {
      return await Task.FromResult(password);
    }

    public bool IsDigit(string password)
    {
      var regex = new Regex("(\\d)+");
      return regex.IsMatch(password);
    }

    public bool IsLowercase(string password)
    {
      var regex = new Regex("[a-z]+");
      return regex.IsMatch(password);
    }

    public bool IsSpecial(string password)
    {
      var regex = new Regex("(\\W)+");
      return regex.IsMatch(password);
    }

    public bool IsUppercase(string password)
    {
      var regex = new Regex("[A-Z]+");
      return regex.IsMatch(password);
    }

    public async Task<bool> IsValidAsync(string password, string passwordHash)
    {
      return await Task.FromResult(password.Equals(passwordHash));
    }
  }
}
