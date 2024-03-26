using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.Common;
using MediatR;

namespace Florists.Application.Features.Auth.Commands.ChangePassword
{
  public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ErrorOr<MessageResultDTO>>
  {
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;

    public ChangePasswordCommandHandler(
      IUserRepository userRepository,
      IPasswordService passwordService)
    {
      _userRepository = userRepository;
      _passwordService = passwordService;
    }

    public async Task<ErrorOr<MessageResultDTO>> Handle(
      ChangePasswordCommand command,
      CancellationToken cancellationToken)
    {
      var user = await _userRepository.GetOneByEmailAsync(command.Email!);

      if (user is null)
      {
        return CustomErrors.Users.NotFound;
      }

      var oldPasswordHash = user.PasswordHash;
      var oldIsPasswordChanged = user.IsPasswordChanged;

      var isOldPasswordValid = _passwordService.IsValid(
        command.Password,
        user.PasswordHash);

      if (!isOldPasswordValid)
      {
        return CustomErrors.Auth.PasswordIsInvalid;
      }

      var newPasswordHash = _passwordService.GenerateHash(command.NewPassword);

      user.IsPasswordChanged = true;
      user.PasswordHash = newPasswordHash;

      var changePasswordSuccess = await _userRepository.ChangePasswordAsync(user);

      if (!changePasswordSuccess)
      {
        user.PasswordHash = oldPasswordHash;
        user.IsPasswordChanged = oldIsPasswordChanged;
        return CustomErrors.Database.SaveError;
      }

      return new MessageResultDTO(
        true,
        Messages.Database.SaveSuccess);
    }
  }
}
