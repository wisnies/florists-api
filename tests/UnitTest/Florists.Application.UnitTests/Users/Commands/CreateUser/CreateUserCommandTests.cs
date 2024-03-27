using Florists.Application.Features.Users.Commands.CreateUser;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Users;
using Florists.Application.UnitTests.Users.Commands.TestUtils;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Users.Commands.CreateUser
{
  public class CreateUserCommandTests
  {
    private CreateUserCommandHandler _handler;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IPasswordService> _mockPasswordService;
    private Mock<IDateTimeService> _mockDateTimeService;

    [SetUp]
    public void SetUp()
    {
      _mockUserRepository = new Mock<IUserRepository>();
      _mockPasswordService = new Mock<IPasswordService>();
      _mockDateTimeService = new Mock<IDateTimeService>();

      _handler = new(
        _mockUserRepository.Object,
        _mockPasswordService.Object,
        _mockDateTimeService.Object);
    }

    [Test]
    public async Task HandleCreateUserCommand_WhenEmailIsTaken_ShouldReturnAuthEmailDuplicateError()
    {
      var command = CreateUserCommandUtils.CreateCommand();
      var dbUser = CreateUserCommandUtils.CreateUser();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Auth.EmailDuplicate));

        _mockUserRepository
          .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());
      });
    }

    [Test]
    public async Task HandleCreateUserCommand_WhenUnableToCreateUserWithValidCommand_ShouldReturnDatabaseSaveError()
    {
      var command = CreateUserCommandUtils.CreateCommand();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)null));

      _mockUserRepository
        .Setup(m => m.CreateAsync(It.IsAny<FloristsUser>()))
        .Returns(Task.FromResult(false));

      _mockPasswordService
        .Setup(m => m.GenerateHash(command.Password))
        .Returns(Constants.Users.PasswordHash);

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Users.UtcNow));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.SaveError));

        _mockUserRepository
          .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());

        _mockUserRepository
       .Verify(m => m.CreateAsync(It.IsAny<FloristsUser>()), Times.Once());

        _mockPasswordService
        .Verify(m => m.GenerateHash(command.Password), Times.Once());

        _mockDateTimeService
        .Verify(m => m.UtcNow, Times.Exactly(2));
      });
    }

    [Test]
    public async Task HandleCreateUserCommand_WhenValidCommand_ShouldReturnUserResultDTO()
    {
      var command = CreateUserCommandUtils.CreateCommand();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(command.Email))
        .Returns(Task.FromResult((FloristsUser?)null));

      _mockUserRepository
        .Setup(m => m.CreateAsync(It.IsAny<FloristsUser>()))
        .Returns(Task.FromResult(true));

      _mockPasswordService
        .Setup(m => m.GenerateHash(command.Password))
        .Returns(Constants.Users.PasswordHash);

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Users.UtcNow));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(command);

        _mockUserRepository
          .Verify(m => m.GetOneByEmailAsync(command.Email), Times.Once());

        _mockUserRepository
       .Verify(m => m.CreateAsync(It.IsAny<FloristsUser>()), Times.Once());

        _mockPasswordService
        .Verify(m => m.GenerateHash(command.Password), Times.Once());

        _mockDateTimeService
        .Verify(m => m.UtcNow, Times.Exactly(2));
      });
    }
  }
}
