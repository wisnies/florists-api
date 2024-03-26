using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.User;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.Users.Commands.EditUser
{
  public class EditUserCommandHandler : IRequestHandler<EditUserCommand, ErrorOr<UserResultDTO>>
  {
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeService _dateTimeService;

    public EditUserCommandHandler(
      IUserRepository userRepository,
      IDateTimeService dateTimeService)
    {
      _userRepository = userRepository;
      _dateTimeService = dateTimeService;
    }

    public async Task<ErrorOr<UserResultDTO>> Handle(
      EditUserCommand command,
      CancellationToken cancellationToken)
    {
      var userToUpdate = await _userRepository.GetOneByIdAsync(command.UserId);

      if (userToUpdate is null || userToUpdate.Role is null)
      {
        return CustomErrors.Users.NotFound;
      }

      userToUpdate.Email = command.Email;
      userToUpdate.FirstName = command.FirstName;
      userToUpdate.LastName = command.LastName;
      userToUpdate.UpdatedAt = _dateTimeService.UtcNow;

      if (userToUpdate.Role.RoleType != command.RoleType)
      {
        if ((RoleTypeOptions)userToUpdate.Role.RoleType == RoleTypeOptions.Admin || command.RoleType == RoleTypeOptions.Admin)
        {
          return CustomErrors.Users.UnauthorizedToModifyAdmin;
        }

        userToUpdate.Role.RoleType = command.RoleType;
        userToUpdate.Role.UpdatedAt = _dateTimeService.UtcNow;

      }

      var userSuccess = await _userRepository.UpdateAsync(userToUpdate);

      if (!userSuccess)
      {
        return CustomErrors.Database.SaveError;
      }

      return new UserResultDTO(
        Messages.Database.UpdateSuccess,
        userToUpdate);
    }
  }
}
