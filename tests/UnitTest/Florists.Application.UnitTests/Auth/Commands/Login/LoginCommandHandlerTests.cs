using Florists.Application.Features.Auth.Commands.Login;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Application.UnitTests.Auth.Commands.TestUtils;
using Florists.Application.UnitTests.TestUtils.Auth.Extenstions;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Auth.Commands.Login
{
  public class LoginCommandHandlerTests
  {
    private LoginCommandHandler _handler;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IPasswordService> _mockPasswordService;
    private Mock<ITokenService> _mockTokenService;

    [SetUp]
    public void Setup()
    {
      _mockUserRepository = new Mock<IUserRepository>();
      _mockPasswordService = new Mock<IPasswordService>();
      _mockTokenService = new Mock<ITokenService>();
      _handler = new(
        _mockUserRepository.Object,
        _mockTokenService.Object,
        _mockPasswordService.Object);
    }

    [Test]
    public async Task HandleLoginCommand_WhenCredentialsAreValid_ShouldAuthenticateAndReturnTokens()
    {
      var loginCommand = LoginCommandUtils.CreateCommand();
      var dbUser = LoginCommandUtils.CreateUser();
      var userTokensDTO = LoginCommandUtils.CreateUserTokensDTO();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(loginCommand.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockUserRepository
        .Setup(m => m.AuthenticateAsync(dbUser, userTokensDTO))
        .Returns(Task.FromResult(true));

      _mockPasswordService
        .Setup(m => m.IsValid(
          loginCommand.Password,
          dbUser.PasswordHash))
        .Returns(true);

      _mockTokenService
        .Setup(m => m.GenerateToken(dbUser))
        .Returns(userTokensDTO);



      var result = await _handler.Handle(
        loginCommand,
        default);


      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(loginCommand);


        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(loginCommand.Email),
          Times.Once());

        _mockUserRepository
        .Verify(
          m => m.AuthenticateAsync(dbUser, userTokensDTO),
          Times.Once());

        _mockPasswordService
        .Verify(m => m.IsValid(
          loginCommand.Password,
          dbUser.PasswordHash), Times.Once());

        _mockTokenService
          .Verify(m => m.GenerateToken(dbUser), Times.Once());
      });
    }

    [Test]
    public async Task HandleLoginCommand_WhenEmailIsNotPresentInDatabase_ShouldReturnInvalidCredentialsError()
    {
      var loginCommand = LoginCommandUtils.CreateCommand();
      FloristsUser? dbUser = null;

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(loginCommand.Email))
        .Returns(Task.FromResult(dbUser));


      var result = await _handler.Handle(
        loginCommand,
        default);


      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Auth.InvalidCredentials));

        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(loginCommand.Email),
          Times.Once());
      });
    }

    [Test]
    public async Task HandleLoginCommand_WhenPasswordIsInvalid_ShouldReturnInvalidCredentialsError()
    {
      var loginCommand = LoginCommandUtils.CreateCommand();
      var dbUser = LoginCommandUtils.CreateUser();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(loginCommand.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockPasswordService
        .Setup(m => m.IsValid(
          loginCommand.Password,
          dbUser.PasswordHash))
        .Returns(false);


      var result = await _handler.Handle(
        loginCommand,
        default);


      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Auth.InvalidCredentials));

        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(loginCommand.Email),
          Times.Once());

        _mockPasswordService
        .Verify(m => m.IsValid(
          loginCommand.Password,
          dbUser.PasswordHash), Times.Once());
      });
    }

    [Test]
    public async Task HandleLoginCommand_WhenUnableToAuthenticateWithValidCredentials_ShouldReturnDatabaseSaveError()
    {
      var loginCommand = LoginCommandUtils.CreateCommand();
      var dbUser = LoginCommandUtils.CreateUser();
      var userTokensDTO = LoginCommandUtils.CreateUserTokensDTO();

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(loginCommand.Email))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockUserRepository
        .Setup(m => m.AuthenticateAsync(dbUser, userTokensDTO))
        .Returns(Task.FromResult(false));

      _mockPasswordService
        .Setup(m => m.IsValid(
          loginCommand.Password,
          dbUser.PasswordHash))
        .Returns(true);

      _mockTokenService
        .Setup(m => m.GenerateToken(dbUser))
        .Returns(userTokensDTO);



      var result = await _handler.Handle(
        loginCommand,
        default);


      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.SaveError));

        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(loginCommand.Email),
          Times.Once());

        _mockUserRepository
        .Verify(
          m => m.AuthenticateAsync(dbUser, userTokensDTO),
          Times.Once());

        _mockPasswordService
        .Verify(m => m.IsValid(
          loginCommand.Password,
          dbUser.PasswordHash), Times.Once());

        _mockTokenService
          .Verify(m => m.GenerateToken(dbUser), Times.Once());
      });
    }

  }
}
