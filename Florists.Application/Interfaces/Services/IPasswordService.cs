namespace Florists.Application.Interfaces.Services
{
  public interface IPasswordService
  {
    public Task<string> GenerateHashAsync(string password);
    public Task<bool> IsValidAsync(
      string password,
      string passwordHash);

    public bool IsLowercase(string password);
    public bool IsUppercase(string password);
    public bool IsDigit(string password);
    public bool IsSpecial(string password);
  }
}
