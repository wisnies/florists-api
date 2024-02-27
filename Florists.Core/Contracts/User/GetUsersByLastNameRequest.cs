namespace Florists.Core.Contracts.User
{
  public record GetUsersByLastNameRequest(
    string? LastName,
    int Page = 1,
    int PerPage = 10);
}
