using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.User;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.Users.Commands.DeleteUser
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
      var userToDelete = await _userRepository.GetOneByIdAsync(command.UserId);

      if (userToDelete is null || userToDelete.Role is null)
      {
        return CustomErrors.Users.NotFound;
      }

      if ((RoleTypeOptions)userToDelete.Role.RoleType == RoleTypeOptions.Admin)
      {
        return CustomErrors.Users.UnauthorizedToModifyAdmin;
      }

      var success = await _userRepository.DeleteSoft(userToDelete);

      if (!success)
      {
        return CustomErrors.Database.SaveError;
      }

      return new UserResultDTO(
        Messages.Database.DeleteSuccess,
        userToDelete);
    }
  }
}
