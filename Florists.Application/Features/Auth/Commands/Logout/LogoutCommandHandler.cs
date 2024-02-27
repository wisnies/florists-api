using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Common;
using MediatR;

namespace Florists.Application.Features.Auth.Commands.Logout
{
  public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ErrorOr<MessageResultDTO>>
  {
    private readonly IUserRepository _userRepository;

    public LogoutCommandHandler(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<ErrorOr<MessageResultDTO>> Handle(
      LogoutCommand command,
      CancellationToken cancellationToken)
    {
      var user = await _userRepository.GetOneByEmailAsync(command.Email!);

      if (user is null)
      {
        return CustomErrors.Auth.InvalidCredentials;
      }

      var success = await _userRepository.LogoutAsync(user);

      if (!success)
      {
        return CustomErrors.Auth.UnableToLogout;
      }

      return new MessageResultDTO(
        true,
        Messages.Auth.LogoutSuccess);
    }
  }
}
