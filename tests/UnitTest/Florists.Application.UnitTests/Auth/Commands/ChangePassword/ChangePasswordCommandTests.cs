using Florists.Application.Features.Auth.Commands.ChangePassword;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Application.UnitTests.Auth.Commands.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Auth;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommandTests
  {
    private ChangePasswordCommandHandler _handler;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IPasswordService> _mockPasswordService;

    [SetUp]
    public void Setup()
    {
      _mockUserRepository = new Mock<IUserRepository>();
      _mockPasswordService = new Mock<IPasswordService>();
      _handler = new(
        _mockUserRepository.Object,
        _mockPasswordService.Object);
    }

    [Test]
    public async Task HandleChangePasswordCommand_WhenRequestIsValid_ShouldReturnMessageResult()
    {
      var changePasswordCommand = ChangePasswordCommandUtils.CreateCommand();
      var dbUser = ChangePasswordCommandUtils.CreateUser();

      var oldPasswordHash = dbUser.PasswordHash;
      var newPasswordHash = Constants.Auth.NewPassword;

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(changePasswordCommand.Email!))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockUserRepository
        .Setup(m => m.ChangePasswordAsync(dbUser))
        .Returns(Task.FromResult(true));

      _mockPasswordService
        .Setup(m => m.IsValid(
          changePasswordCommand.Password,
          dbUser.PasswordHash))
        .Returns(true);

      _mockPasswordService
        .Setup(m => m.GenerateHash(changePasswordCommand.NewPassword))
        .Returns(newPasswordHash);

      var result = await _handler.Handle(
        changePasswordCommand,
      default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        Assert.That(dbUser.PasswordHash, Is.EqualTo(newPasswordHash));
        Assert.That(dbUser.IsPasswordChanged, Is.True);
        result.Value.ValidateCreatedFrom(changePasswordCommand);

        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(changePasswordCommand.Email!),
        Times.Once());

        _mockUserRepository
        .Verify(
          m => m.ChangePasswordAsync(dbUser),
          Times.Once());

        _mockPasswordService
        .Verify(
          m => m.IsValid(
          changePasswordCommand.Password,
          oldPasswordHash),
          Times.Once());

        _mockPasswordService
        .Verify(
          m => m.GenerateHash(changePasswordCommand.NewPassword),
          Times.Once());
      });
    }

    [Test]
    public async Task HandleChangePasswordCommand_WhenCredentialsAreInvalid_ShouldReturnUserNotFoundError()
    {
      var changePasswordCommand = ChangePasswordCommandUtils.CreateCommand();
      FloristsUser? dbUser = null;

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(changePasswordCommand.Email!))
        .Returns(Task.FromResult(dbUser));

      var result = await _handler.Handle(
        changePasswordCommand,
      default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Users.NotFound));

        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(changePasswordCommand.Email!),
        Times.Once());
      });
    }

    [Test]
    public async Task HandleChangePasswordCommand_WhenOldPasswordIsInvalid_ShouldReturnPasswordIsInvalidError()
    {
      var changePasswordCommand = ChangePasswordCommandUtils.CreateCommand();
      var dbUser = ChangePasswordCommandUtils.CreateUser();

      var oldPasswordHash = dbUser.PasswordHash;

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(changePasswordCommand.Email!))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockPasswordService
        .Setup(m => m.IsValid(
          changePasswordCommand.Password,
          dbUser.PasswordHash))
        .Returns(false);

      var result = await _handler.Handle(
        changePasswordCommand,
      default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(dbUser.PasswordHash, Is.EqualTo(oldPasswordHash));
        Assert.That(dbUser.IsPasswordChanged, Is.False);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Auth.PasswordIsInvalid));

        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(changePasswordCommand.Email!),
        Times.Once());

        _mockPasswordService
        .Verify(
          m => m.IsValid(
          changePasswordCommand.Password,
          oldPasswordHash),
          Times.Once());
      });
    }

    [Test]
    public async Task HandleChangePasswordCommand_WhenUnableToChangePasswordWithValidCredentials_ShouldReturnDatabaseSaveError()
    {
      var changePasswordCommand = ChangePasswordCommandUtils.CreateCommand();
      var dbUser = ChangePasswordCommandUtils.CreateUser();

      var oldPasswordHash = dbUser.PasswordHash;
      var newPasswordHash = Constants.Auth.NewPassword;

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(changePasswordCommand.Email!))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockUserRepository
        .Setup(m => m.ChangePasswordAsync(dbUser))
        .Returns(Task.FromResult(false));

      _mockPasswordService
        .Setup(m => m.IsValid(
          changePasswordCommand.Password,
          dbUser.PasswordHash))
        .Returns(true);

      _mockPasswordService
        .Setup(m => m.GenerateHash(changePasswordCommand.NewPassword))
        .Returns(newPasswordHash);

      var result = await _handler.Handle(
        changePasswordCommand,
      default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(dbUser.PasswordHash, Is.EqualTo(oldPasswordHash));
        Assert.That(dbUser.IsPasswordChanged, Is.False);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.SaveError));

        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(changePasswordCommand.Email!),
        Times.Once());

        _mockUserRepository
        .Verify(
          m => m.ChangePasswordAsync(dbUser),
          Times.Once());

        _mockPasswordService
        .Verify(
          m => m.IsValid(
          changePasswordCommand.Password,
          oldPasswordHash),
          Times.Once());

        _mockPasswordService
        .Verify(
          m => m.GenerateHash(changePasswordCommand.NewPassword),
          Times.Once());
      });
    }

  }
}
