namespace Florists.Core.Contracts.Auth
{
  public record ChangePasswordRequest(
    string Password,
    string NewPassword,
    string ConfirmNewPassword);
}
