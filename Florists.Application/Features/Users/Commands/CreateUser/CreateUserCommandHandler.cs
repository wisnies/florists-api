using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.User;
using Florists.Core.Entities;
using MediatR;

namespace Florists.Application.Features.Users.Commands.CreateUser
{
  public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<UserResultDTO>>
  {
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IDateTimeService _dateTimeService;

    public CreateUserCommandHandler(
      IUserRepository userRepository,
      IPasswordService passwordService,
      IDateTimeService dateTimeService)
    {
      _userRepository = userRepository;
      _passwordService = passwordService;
      _dateTimeService = dateTimeService;
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

      var role = new FloristsRole
      {
        RoleId = Guid.NewGuid(),
        UserId = user.UserId,
        RoleType = command.RoleType,
        CreatedAt = _dateTimeService.UtcNow,
      };

      user.Role = role;

      var success = await _userRepository.CreateAsync(user);

      if (!success)
      {
        return CustomErrors.Database.SaveError;
      }

      return new UserResultDTO(
        Messages.Database.SaveSuccess,
        user);
    }
  }
}
