using ErrorOr;
using Florists.Core.DTO.User;
using MediatR;

namespace Florists.Application.Features.User.Commands.DeleteUser
{
  public record DeleteUserCommand(Guid UserId) : IRequest<ErrorOr<UserResultDTO>>;
}
