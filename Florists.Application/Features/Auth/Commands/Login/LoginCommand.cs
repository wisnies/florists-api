using ErrorOr;
using Florists.Core.DTO.Auth;
using MediatR;

namespace Florists.Application.Features.Auth.Commands.Login
{
  public record LoginCommand(
    string Email,
    string Password) : IRequest<ErrorOr<AuthResultDTO>>;
}
