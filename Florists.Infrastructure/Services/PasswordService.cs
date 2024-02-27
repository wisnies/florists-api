using Florists.Application.Interfaces.Services;

namespace Florists.Infrastructure.Services
{
  public class PasswordService : IPasswordService
  {
    public async Task<string> GenerateHashAsync(string password)
    {
      return await Task.FromResult(password);
    }

    public async Task<bool> IsValidAsync(string password, string passwordHash)
    {
      return await Task.FromResult(password.Equals(passwordHash));
    }
  }
}
