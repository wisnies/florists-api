using Florists.Application.Features.Users.Queries.GetUserById;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.UnitTests.TestUtils.Extenstions.Users;
using Florists.Application.UnitTests.Users.Queries.TestUtils;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Users.Queries.GetUserById
{
  public class GetUserByIdQueryTests
  {
    private GetUserByIdQueryHandler _handler;
    private Mock<IUserRepository> _mockUserRepository;

    [SetUp]
    public void SetUp()
    {
      _mockUserRepository = new Mock<IUserRepository>();
      _handler = new(_mockUserRepository.Object);
    }

    [Test]
    public async Task HandleGetUserByIdQuery_WhenUserNotFoundById_ShouldReturnUserNotFoundError()
    {
      var query = GetUserByIdQueryUtils.CreateQuery();

      _mockUserRepository
        .Setup(m => m.GetOneByIdAsync(query.UserId))
        .Returns(Task.FromResult((FloristsUser?)null));

      var result = await _handler.Handle(query, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Users.NotFound));

        _mockUserRepository
       .Verify(m => m.GetOneByIdAsync(query.UserId), Times.Once());
      });
    }

    [Test]
    public async Task HandleGetUserByIdQuery_WhenValidQuery_ShouldReturnUserRFesultDTO()
    {
      var query = GetUserByIdQueryUtils.CreateQuery();
      var dbUser = GetUserByIdQueryUtils.CreateUser();

      _mockUserRepository
        .Setup(m => m.GetOneByIdAsync(query.UserId))
        .Returns(Task.FromResult((FloristsUser?)dbUser));

      var result = await _handler.Handle(query, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(query);

        _mockUserRepository
       .Verify(m => m.GetOneByIdAsync(query.UserId), Times.Once());
      });
    }
  }
}
