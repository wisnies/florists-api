using ErrorOr;
using Florists.Core.DTO.User;
using MediatR;

namespace Florists.Application.Features.User.Queries.GetUserById
{
  public record GetUserByIdQuery(Guid UserId) : IRequest<ErrorOr<UserResultDTO>>;

}
