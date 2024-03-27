using Florists.Application.Features.Users.Queries.GetUsersByLastName;
using Florists.Application.Interfaces.Persistence;
using Florists.Application.UnitTests.TestUtils.Constants;
using Florists.Application.UnitTests.TestUtils.Extenstions.Users;
using Florists.Application.UnitTests.Users.Queries.TestUtils;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Entities;
using Moq;

namespace Florists.Application.UnitTests.Users.Queries.GetUsersByLastName
{
  public class GetUsersByLastNameTests
  {
    private GetUsersByLastNameQueryHandler _handler;
    private Mock<IUserRepository> _mockUserRepository;

    [SetUp]
    public void SetUp()
    {
      _mockUserRepository = new Mock<IUserRepository>();
      _handler = new(_mockUserRepository.Object);
    }

    public async Task HandleGetUsersByLastNameQuery_WhenUnableToFetchWithValidQuery_ShouldReturnDatabaseSaveError()
    {
      var query = GetUsersByLastNameQueryUtils.CreateQuery();
      var offset = query.PerPage * (query.Page - 1);

      _mockUserRepository
        .Setup(m => m.GetManyByLastNameAsync(offset, query.PerPage, query.LastName))
        .Returns(Task.FromResult((List<FloristsUser>?)null));

      var result = await _handler.Handle(query, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.True);
        Assert.That(result.FirstError, Is.EqualTo(CustomErrors.Database.FetchError));

        _mockUserRepository
          .Verify(m => m.GetManyByLastNameAsync(offset, query.PerPage, query.LastName), Times.Once());
      });
    }

    [TestCase(1, 1)]
    [TestCase(2, 2)]
    [TestCase(5, 1)]
    [TestCase(3, 10)]
    [TestCase(123, 1)]
    public async Task HandleGetUsersByLastNameQuery_WhenValidQuery_ShouldReturnUsersResultDTOWIthValidCollectionLength(int perPage, int page)
    {
      var query = GetUsersByLastNameQueryUtils.CreateQuery(perPage, page);
      var offset = query.PerPage * (query.Page - 1);
      var dbUsers = GetUsersByLastNameQueryUtils.CreateUsers(offset, perPage);


      _mockUserRepository
        .Setup(m => m.GetManyByLastNameAsync(offset, query.PerPage, query.LastName))
        .Returns(Task.FromResult((List<FloristsUser>?)dbUsers));

      _mockUserRepository
        .Setup(m => m.CountByLastNameAsync(query.LastName))
        .Returns(Task.FromResult(Constants.Users.UsersCount));

      var result = await _handler.Handle(query, default);

      Assert.Multiple(() =>
      {
        Assert.That(result.IsError, Is.False);
        result.Value.ValidateCreatedFrom(query, offset);

        _mockUserRepository
          .Verify(m => m.GetManyByLastNameAsync(offset, query.PerPage, query.LastName), Times.Once());

        _mockUserRepository
          .Verify(m => m.CountByLastNameAsync(query.LastName), Times.Once());
      });
    }
  }
}
