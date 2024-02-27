using ErrorOr;
using Florists.Core.DTO.User;
using MediatR;

namespace Florists.Application.Features.User.Queries.GetUsersByLastName
{
  public record GetUsersByLastNameQuery(
    string LastName,
    int Page,
    int PerPage) : IRequest<ErrorOr<UsersResultDTO>>;
}
