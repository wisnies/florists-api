using ErrorOr;
using Florists.Core.DTO.User;
using MediatR;

namespace Florists.Application.Features.Users.Queries.GetUserById
{
  public record GetUserByIdQuery(Guid UserId) : IRequest<ErrorOr<UserResultDTO>>;

}
