using ErrorOr;
using Florists.Core.DTO.Common;
using MediatR;

namespace Florists.Application.Features.Auth.Commands.ChangePassword
{
  public record ChangePasswordCommand(
    string Password,
    string NewPassword,
    string ConfirmNewPassword,
    string? Email) : IRequest<ErrorOr<MessageResultDTO>>;
}
