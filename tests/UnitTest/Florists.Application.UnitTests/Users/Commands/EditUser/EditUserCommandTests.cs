using Florists.Application.Features.Users.Commands.EditUser;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.Interfaces.Services;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Users;
using Florists.Application.UnitTests.Users.Commands.TestUtils;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Users.Commands.EditUser
{
  public class EditUserCommandTests
  {
    private EditUserCommandHandler _handler;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IDateTimeService> _mockDateTimeService;

    [SetUp]
    public void SetUp()
    {
      _mockUserRepository = new Mock<IUserRepository>();
      _mockDateTimeService = new Mock<IDateTimeService>();

      _handler = new(
        _mockUserRepository.Object,
        _mockDateTimeService.Object);
    }

    [Test]
    public async Task HandleEditUserCommand_WhenUserNotFoundById_ShouldReturnUserNotFoundError()
    {
      var command = EditUserCommandUtils.CreateCommand();

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
    public async Task HandleEditUserCommand_WhenUserIsAdmin_ShouldReturnUserNotAuthorizedToModifyAdminError()
    {
      var command = EditUserCommandUtils.CreateCommand();
      var dbUser = EditUserCommandUtils.CreateUser(true);

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
    public async Task HandleEditUserCommand_WhenChangingRoleToAdmin_ShouldReturnUserNotAuthorizedToModifyAdminError()
    {
      var command = EditUserCommandUtils.CreateCommand(true);
      var dbUser = EditUserCommandUtils.CreateUser();

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
    public async Task HandleEditUserCommand_WhenUnableToEditWithValidCommand_ShouldReturnDatabaseSaveError()
    {
      var command = EditUserCommandUtils.CreateCommand();
      var dbUser = EditUserCommandUtils.CreateUser();

      _mockUserRepository
         .Setup(m => m.GetOneByIdAsync(command.UserId))
         .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockUserRepository
         .Setup(m => m.UpdateAsync(It.IsAny<FloristsUser>()))
         .Returns(Task.FromResult(false));

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Users.EditedUtcNow));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.SaveError));

        _mockUserRepository
          .Verify(m => m.GetOneByIdAsync(command.UserId), Times.Once());

        _mockUserRepository
         .Verify(m => m.UpdateAsync(It.IsAny<FloristsUser>()), Times.Once());

        _mockDateTimeService
        .Verify(m => m.UtcNow, Times.Exactly(2));
      });
    }

    [Test]
    public async Task HandleEditUserCommand_WhenValidCommand_ShouldReturnUserResultDTO()
    {
      var command = EditUserCommandUtils.CreateCommand();
      var dbUser = EditUserCommandUtils.CreateUser();

      _mockUserRepository
         .Setup(m => m.GetOneByIdAsync(command.UserId))
         .Returns(Task.FromResult((FloristsUser?)dbUser));

      _mockUserRepository
         .Setup(m => m.UpdateAsync(It.IsAny<FloristsUser>()))
         .Returns(Task.FromResult(true));

      _mockDateTimeService
        .Setup(m => m.UtcNow)
        .Returns(DateTime.Parse(Constants.Users.EditedUtcNow));

      var result = await _handler.Handle(command, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(command);

        _mockUserRepository
          .Verify(m => m.GetOneByIdAsync(command.UserId), Times.Once());

        _mockUserRepository
         .Verify(m => m.UpdateAsync(It.IsAny<FloristsUser>()), Times.Once());

        _mockDateTimeService
        .Verify(m => m.UtcNow, Times.Exactly(2));
      });
    }
  }
}
