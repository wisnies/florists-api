using ErrorOr;
using Florists.Core.DTO.User;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.User.Commands.CreateUser
{
  public record CreateUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string ConfirmPassword,
    RoleTypeOptions RoleType) : IRequest<ErrorOr<UserResultDTO>>;
}
