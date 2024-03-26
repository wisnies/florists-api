namespace Florists.Core.Contracts.User
{
  public record GetUsersByLastNameRequest(
    string? LastName,
    int Page,
    int PerPage);
}
