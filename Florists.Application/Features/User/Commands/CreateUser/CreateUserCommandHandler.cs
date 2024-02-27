using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.User;
using Florists.Core.Entities;
using MediatR;

namespace Florists.Application.Features.User.Commands.CreateUser
{
  public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<UserResultDTO>>
  {
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordService _passwordService;
    private readonly IDateTimeService _dateTimeService;

    public CreateUserCommandHandler(
      IUserRepository userRepository,
      IPasswordService passwordService,
      IDateTimeService dateTimeService,
      IRoleRepository roleRepository)
    {
      _userRepository = userRepository;
      _passwordService = passwordService;
      _dateTimeService = dateTimeService;
      _roleRepository = roleRepository;
    }

    public async Task<ErrorOr<UserResultDTO>> Handle(
      CreateUserCommand command,
      CancellationToken cancellationToken)
    {
      var dbUser = await _userRepository.GetOneByEmailAsync(command.Email);

      if (dbUser is not null)
      {
        return CustomErrors.Auth.EmailDuplicate;
      }

      var passwordHash = await _passwordService.GenerateHashAsync(command.Password);

      var user = new FloristsUser
      {
        UserId = Guid.NewGuid(),
        IsActive = true,
        IsPasswordChanged = false,
        Email = command.Email,
        FirstName = command.FirstName,
        LastName = command.LastName,
        PasswordHash = passwordHash,
        CreatedAt = _dateTimeService.UtcNow,
      };

      var userSuccess = await _userRepository.CreateAsync(user);

      if (!userSuccess)
      {
        return CustomErrors.User.UnableToCreateUser;
      }

      var role = new FloristsRole
      {
        RoleId = Guid.NewGuid(),
        UserId = user.UserId,
        RoleType = command.RoleType,
        CreatedAt = _dateTimeService.UtcNow,
      };

      var roleSuccess = await _roleRepository.CreateAsync(role);

      if (!roleSuccess)
      {
        await _userRepository.Delete(user);
        return CustomErrors.User.UnableToCreateUserRole;
      }

      user.Role = role;

      return new UserResultDTO(Messages.User.CreateSuccess, user);
    }
  }
}
