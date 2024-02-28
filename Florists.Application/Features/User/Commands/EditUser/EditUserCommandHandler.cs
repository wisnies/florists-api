using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.User;
using Florists.Core.Enums;
using MediatR;

namespace Florists.Application.Features.User.Commands.EditUser
{
  public class EditUserCommandHandler : IRequestHandler<EditUserCommand, ErrorOr<UserResultDTO>>
  {
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IDateTimeService _dateTimeService;

    public EditUserCommandHandler(
      IUserRepository userRepository,
      IRoleRepository roleRepository,
      IDateTimeService dateTimeService)
    {
      _userRepository = userRepository;
      _roleRepository = roleRepository;
      _dateTimeService = dateTimeService;
    }

    public async Task<ErrorOr<UserResultDTO>> Handle(
      EditUserCommand command,
      CancellationToken cancellationToken)
    {
      var user = await _userRepository.GetOneByIdAsync(command.UserId);

      if (user is null || user.Role is null)
      {
        return CustomErrors.User.NotFound;
      }



      user.Email = command.Email;
      user.FirstName = command.FirstName;
      user.LastName = command.LastName;
      user.UpdatedAt = _dateTimeService.UtcNow;

      var currentUserRole = user.Role.RoleType;

      if (user.Role.RoleType != command.RoleType)
      {
        if ((RoleTypeOptions)user.Role.RoleType == RoleTypeOptions.Admin)
        {
          return CustomErrors.User.UnableToEditAdminRole;
        }

        user.Role.RoleType = command.RoleType;
        user.Role.UpdatedAt = _dateTimeService.UtcNow;

        var roleSuccess = await _roleRepository.UpdateAsync(user.Role);

        if (!roleSuccess)
        {
          return CustomErrors.User.UnableToUpdateUserRole;
        }
      }

      var userSuccess = await _userRepository.UpdateAsync(user);

      if (!userSuccess)
      {
        user.Role.RoleType = currentUserRole;
        await _roleRepository.UpdateAsync(user.Role);
        return CustomErrors.User.UnableToUpdateUser;
      }

      return new UserResultDTO(Messages.User.UpdateSuccess, user);
    }
  }
}
