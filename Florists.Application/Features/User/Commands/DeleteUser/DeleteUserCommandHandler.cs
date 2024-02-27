using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.User;
using MediatR;

namespace Florists.Application.Features.User.Commands.DeleteUser
{
  public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ErrorOr<UserResultDTO>>
  {
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<ErrorOr<UserResultDTO>> Handle(
      DeleteUserCommand command,
      CancellationToken cancellationToken)
    {
      var user = await _userRepository.GetOneByIdAsync(command.UserId);

      if (user is null)
      {
        return CustomErrors.User.NotFound;
      }

      var success = await _userRepository.DeleteSoft(user);

      if (!success)
      {
        return CustomErrors.User.UnableToDeleteUser;
      }

      return new UserResultDTO(Messages.User.DeleteSuccess, user);
    }
  }
}
