using ErrorOr;
using Florists.Core.DTO.User;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.Users.Commands.EditUser
{
  public record EditUserCommand(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    RoleTypeOptions RoleType) : IRequest<ErrorOr<UserResultDTO>>;
}
