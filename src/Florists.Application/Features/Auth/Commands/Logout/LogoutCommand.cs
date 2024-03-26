using ErrorOr;
using Florists.Core.DTO.Common;
using MediatR;

namespace Florists.Application.Features.Auth.Commands.Logout
{
  public record LogoutCommand(string? Email) : IRequest<ErrorOr<MessageResultDTO>>;
}
