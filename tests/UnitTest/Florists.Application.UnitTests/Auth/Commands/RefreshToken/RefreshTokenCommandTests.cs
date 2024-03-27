using Florists.Application.Features.Auth.Commands.RefreshToken;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Application.UnitTests.Auth.Commands.TestUtils;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Auth;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;
using System.Security.Claims;

namespace Florists.Application.UnitTests.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandTests
  {
    private RefreshTokenCommandHandler _handler;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IDateTimeService> _mockDateTimeService;
    private Mock<ITokenService> _mockTokenService;

    [SetUp]
    public void Setup()
    {
      _mockUserRepository = new Mock<IUserRepository>();
      _mockDateTimeService = new Mock<IDateTimeService>();
      _mockTokenService = new Mock<ITokenService>();
      _handler = new(
        _mockUserRepository.Object,
        _mockTokenService.Object,
        _mockDateTimeService.Object);
    }

    [Test]
    public async Task HandleRefreshTokenCommand_WhenTokensAreValid_ShouldAuthenticateAndReturnTokens()
    {
      var refreshTokenCommand = RefreshTokenCommandUtils.CreateCommand();
      var dbUser = RefreshTokenCommandUtils.CreateUser();
      var claimsPrincipal = RefreshTokenCommandUtils.CreateClaimsPrincipal(dbUser);
      var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
      var userTokensDTO = RefreshTokenCommandUtils.CreateUserTokensDTO();

      _mockTokenService
        .Setup(m => m.GetClaimsPrincipal(refreshTokenCommand.JwtToken))
        .Returns(claimsPrincipal);

      _mockTokenService
        .Setup(m => m.GenerateToken(dbUser))
        .Returns(userTokensDTO);

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(email!))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockUserRepository
        .Setup(m => m.AuthenticateAsync(dbUser, userTokensDTO))
        .Returns(Task.FromResult(true));

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Auth.ValidUtcNow));

      var result = await _handler.Handle(
        refreshTokenCommand,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(refreshTokenCommand);

        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(email!),
          Times.Once());

        _mockUserRepository
        .Verify(
          m => m.AuthenticateAsync(dbUser, userTokensDTO),
          Times.Once());

        _mockTokenService
          .Verify(m => m.GenerateToken(dbUser), Times.Once());

        _mockTokenService
          .Verify(m => m.GetClaimsPrincipal(refreshTokenCommand.JwtToken), Times.Once());
      });
    }

    [Test]
    public async Task HandleRefreshTokenCommand_WhenClaimsPrincipalFromTokensIsNull_ShouldReturnInvalidCredentialsError()
    {
      var refreshTokenCommand = RefreshTokenCommandUtils.CreateCommand();
      ClaimsPrincipal? claimsPrincipal = null;

      _mockTokenService
        .Setup(m => m.GetClaimsPrincipal(refreshTokenCommand.JwtToken))
        .Returns(claimsPrincipal);

      var result = await _handler.Handle(
        refreshTokenCommand,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(
          result.FirstError,
          Is.EqualTo(CustomErrors.Auth.InvalidCredentials));

        _mockTokenService
          .Verify(
          m => m.GetClaimsPrincipal(refreshTokenCommand.JwtToken),
          Times.Once());
      });
    }

    [Test]
    public async Task HandleRefreshTokenCommand_WhenEmailNotPresentInClaimsPrincipal_ShouldReturnInvalidCredentialsError()
    {
      var refreshTokenCommand = RefreshTokenCommandUtils.CreateCommand();
      var dbUser = RefreshTokenCommandUtils.CreateUser();
      var claimsPrincipal = RefreshTokenCommandUtils.CreateClaimsPrincipal(dbUser, false);
      var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

      _mockTokenService
        .Setup(m => m.GetClaimsPrincipal(refreshTokenCommand.JwtToken))
        .Returns(claimsPrincipal);

      var result = await _handler.Handle(
        refreshTokenCommand,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(email, Is.Null);
        Assert.That(result.IsError, Is.True);
        Assert.That(
          result.FirstError,
          Is.EqualTo(CustomErrors.Auth.InvalidCredentials));

        _mockTokenService
          .Verify(m => m.GetClaimsPrincipal(refreshTokenCommand.JwtToken), Times.Once());
      });
    }

    [Test]
    public async Task HandleRefreshTokenCommand_WhenEmailFromClaimsPrincipalIsInvalid_ShouldReturnUserNotFoundError()
    {
      var refreshTokenCommand = RefreshTokenCommandUtils.CreateCommand();
      var dbUser = RefreshTokenCommandUtils.CreateUser();
      var claimsPrincipal = RefreshTokenCommandUtils.CreateClaimsPrincipal(dbUser);
      var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

      _mockTokenService
        .Setup(m => m.GetClaimsPrincipal(refreshTokenCommand.JwtToken))
        .Returns(claimsPrincipal);

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(email!))
        .Returns(Task.FromResult((FloristsUser?)null));


      var result = await _handler.Handle(
        refreshTokenCommand,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(
          result.FirstError,
          Is.EqualTo(CustomErrors.Users.NotFound));

        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(email!),
          Times.Once());

        _mockTokenService
          .Verify(m => m.GetClaimsPrincipal(refreshTokenCommand.JwtToken), Times.Once());
      });
    }

    [Test]
    public async Task HandleRefreshTokenCommand_WhenRefreshTokenFromRequestIsInvalid_ShouldReturnInvalidCredentialsError()
    {
      var refreshTokenCommand = RefreshTokenCommandUtils.CreateCommand(false);
      var dbUser = RefreshTokenCommandUtils.CreateUser();
      var claimsPrincipal = RefreshTokenCommandUtils.CreateClaimsPrincipal(dbUser);
      var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

      _mockTokenService
        .Setup(m => m.GetClaimsPrincipal(refreshTokenCommand.JwtToken))
        .Returns(claimsPrincipal);

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(email!))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      var result = await _handler.Handle(
        refreshTokenCommand,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(
          result.FirstError,
          Is.EqualTo(CustomErrors.Auth.InvalidCredentials));

        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(email!),
          Times.Once());

        _mockTokenService
          .Verify(m => m.GetClaimsPrincipal(refreshTokenCommand.JwtToken), Times.Once());
      });
    }

    [Test]
    public async Task HandleRefreshTokenCommand_WhenRefreshTokenIsExpired_ShouldReturnInvalidCredentialsError()
    {
      var refreshTokenCommand = RefreshTokenCommandUtils.CreateCommand();
      var dbUser = RefreshTokenCommandUtils.CreateUser(true);
      var claimsPrincipal = RefreshTokenCommandUtils.CreateClaimsPrincipal(dbUser);
      var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

      _mockTokenService
        .Setup(m => m.GetClaimsPrincipal(refreshTokenCommand.JwtToken))
        .Returns(claimsPrincipal);

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(email!))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Auth.ValidRefreshTokenExpiration));

      var result = await _handler.Handle(
        refreshTokenCommand,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(
          result.FirstError,
          Is.EqualTo(CustomErrors.Auth.InvalidCredentials));

        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(email!),
          Times.Once());

        _mockTokenService
          .Verify(m => m.GetClaimsPrincipal(refreshTokenCommand.JwtToken), Times.Once());
      });
    }

    [Test]
    public async Task HandleRefreshTokenCommand_WhenUnableToAuthenticateWithValidTokens_ShouldReturnDatabaseSaveError()
    {
      var refreshTokenCommand = RefreshTokenCommandUtils.CreateCommand();
      var dbUser = RefreshTokenCommandUtils.CreateUser();
      var claimsPrincipal = RefreshTokenCommandUtils.CreateClaimsPrincipal(dbUser);
      var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
      var userTokensDTO = RefreshTokenCommandUtils.CreateUserTokensDTO();

      _mockTokenService
        .Setup(m => m.GetClaimsPrincipal(refreshTokenCommand.JwtToken))
        .Returns(claimsPrincipal);

      _mockTokenService
        .Setup(m => m.GenerateToken(dbUser))
        .Returns(userTokensDTO);

      _mockUserRepository
        .Setup(m => m.GetOneByEmailAsync(email!))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockUserRepository
        .Setup(m => m.AuthenticateAsync(dbUser, userTokensDTO))
        .Returns(Task.FromResult(false));

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Auth.ValidUtcNow));

      var result = await _handler.Handle(
        refreshTokenCommand,
        default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(
          result.FirstError,
          Is.EqualTo(CustomErrors.Database.SaveError));

        _mockUserRepository.Verify(
          m => m.GetOneByEmailAsync(email!),
          Times.Once());

        _mockUserRepository
        .Verify(
          m => m.AuthenticateAsync(dbUser, userTokensDTO),
          Times.Once());

        _mockTokenService
          .Verify(m => m.GenerateToken(dbUser), Times.Once());

        _mockTokenService
          .Verify(m => m.GetClaimsPrincipal(refreshTokenCommand.JwtToken), Times.Once());
      });
    }
  }
}

