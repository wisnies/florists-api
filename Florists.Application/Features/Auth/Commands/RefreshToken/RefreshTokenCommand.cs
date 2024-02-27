using ErrorOr;
using Florists.Core.DTO.Auth;
using MediatR;

namespace Florists.Application.Features.Auth.Commands.RefreshToken
{
  public record RefreshTokenCommand(
    string JwtToken,
    string RefreshToken) : IRequest<ErrorOr<AuthResultDTO>>;
}
