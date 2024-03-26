using Florists.Application.Features.Auth.Commands.Logout;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.UnitTests.Auth.Commands.TestUtils;
using Florists.Application.UnitTests.TestUtils.Auth.Extenstions;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Auth.Commands.Logout
{
  public class LogoutCommandHandlerTests
  {
    private LogoutCommandHandler _handler;
    private Mock<IUserRepository> _mockUserRepository;

    [SetUp]
    public void Setup()
    {
      _mockUserRepository = new Mock<IUserRepository>();
      _handler = new(_mockUserRepository.Object);
    }

    [Test]
    public async Task HandleLogoutCommand_WhenCredentialsAreValid_ShouldReturnMessageResult()
    {
      var logoutCommand = LogoutCommandUtils.CreateCommand();
      var dbUser = LogoutCommandUtils.CreateUser();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(logoutCommand.Email!))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockUserRepository
        .Setup(m => m.LogoutAsync(dbUser))
        .Returns(Task.FromResult(true));

      var result = await _handler.Handle(
        logoutCommand,
      default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(logoutCommand);

        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(logoutCommand.Email!),
        Times.Once());

        _mockUserRepository
        .Verify(
          m => m.LogoutAsync(dbUser),
          Times.Once());
      });
    }

    [Test]
    public async Task HandleLogoutCommand_WhenCredentialsAreInvalid_ShouldReturnUserNotFoundError()
    {
      var logoutCommand = LogoutCommandUtils.CreateCommand();
      FloristsUser? dbUser = null;

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(logoutCommand.Email!))
        .Returns(Task.FromResult(dbUser));

      var result = await _handler.Handle(
        logoutCommand,
      default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Users.NotFound));

        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(logoutCommand.Email!),
        Times.Once());
      });
    }

    [Test]
    public async Task HandleLogoutCommand_WhenUnableToLogoutWithValidCredentials_ShouldReturnDatabaseSaveError()
    {
      var logoutCommand = LogoutCommandUtils.CreateCommand();
      var dbUser = LogoutCommandUtils.CreateUser();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(logoutCommand.Email!))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockUserRepository
        .Setup(m => m.LogoutAsync(dbUser))
        .Returns(Task.FromResult(false));

      var result = await _handler.Handle(
        logoutCommand,
      default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.SaveError));


        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(logoutCommand.Email!),
        Times.Once());

        _mockUserRepository
        .Verify(
          m => m.LogoutAsync(dbUser),
          Times.Once());
      });
    }
  }
}
