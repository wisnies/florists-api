using Florists.Application.Features.Users.Commands.DeleteUser;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.UnitTests.TestUtils.Extenstions.Users;
using Florists.Application.UnitTests.Users.Commands.TestUtils;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Users.Commands.CreateUser
{
  public class DeleteUserCommandTests
  {
    private DeleteUserCommandHandler _handler;
    private Mock<IUserRepository> _mockUserRepository;

    [SetUp]
    public void SetUp()
    {
      _mockUserRepository = new Mock<IUserRepository>();

      _handler = new(_mockUserRepository.Object);
    }

    [Test]
    public async Task HandleDeleteUserCommand_WhenUserNotFoundById_ShouldReturnUserNotFoundError()
    {
      var command = DeleteUserCommandUtils.CreateCommand();

      _mockUserRepository
        .Setup(m => m.GetOneByIdAsync(command.UserId))
        .Returns(Task.FromResult((FloristsUser?)null));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Users.NotFound));

        _mockUserRepository
          .Verify(m => m.GetOneByIdAsync(command.UserId), Times.Once());
      });
    }

    [Test]
    public async Task HandleDeleteUserCommand_WhenUserIsAdmin_ShouldReturnUserNotAuthorizedToModifyAdminError()
    {
      var command = DeleteUserCommandUtils.CreateCommand();
      var dbUser = DeleteUserCommandUtils.CreateUser(true);

      _mockUserRepository
        .Setup(m => m.GetOneByIdAsync(command.UserId))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Users.UnauthorizedToModifyAdmin));

        _mockUserRepository
          .Verify(m => m.GetOneByIdAsync(command.UserId), Times.Once());
      });
    }

    [Test]
    public async Task HandleDeleteUserCommand_WhenUnableToDeleteWithValidCommand_ShouldReturnDatabaseSaveError()
    {
      var command = DeleteUserCommandUtils.CreateCommand();
      var dbUser = DeleteUserCommandUtils.CreateUser();

      _mockUserRepository
        .Setup(m => m.GetOneByIdAsync(command.UserId))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockUserRepository
        .Setup(m => m.DeleteSoft(dbUser))
        .Returns(Task.FromResult(false));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.SaveError));

        _mockUserRepository
          .Verify(m => m.GetOneByIdAsync(command.UserId), Times.Once());

        _mockUserRepository
        .Verify(m => m.DeleteSoft(dbUser), Times.Once());
      });
    }

    [Test]
    public async Task HandleDeleteUserCommand_WhenValidCommand_ShouldReturnUserResultDTO()
    {
      var command = DeleteUserCommandUtils.CreateCommand();
      var dbUser = DeleteUserCommandUtils.CreateUser();

      _mockUserRepository
        .Setup(m => m.GetOneByIdAsync(command.UserId))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockUserRepository
        .Setup(m => m.DeleteSoft(dbUser))
        .Returns(Task.FromResult(true));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(command);

        _mockUserRepository
          .Verify(m => m.GetOneByIdAsync(command.UserId), Times.Once());

        _mockUserRepository
        .Verify(m => m.DeleteSoft(dbUser), Times.Once());
      });
    }
  }
}
