namespace Florists.Application.Interfaces.Services
{
  public interface IPasswordService
  {
    public Task<string> GenerateHashAsync(string password);
    public Task<bool> IsValidAsync(
      string password,
      string passwordHash);
  }
}
