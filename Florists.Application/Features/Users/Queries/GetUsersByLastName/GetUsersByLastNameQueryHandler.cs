using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.User;
using MediatR;

namespace Florists.Application.Features.Users.Queries.GetUsersByLastName
{
  public class GetUsersByLastNameQueryHandler : IRequestHandler<GetUsersByLastNameQuery, ErrorOr<UsersResultDTO>>
  {
    private readonly IUserRepository _userRepository;

    public GetUsersByLastNameQueryHandler(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<ErrorOr<UsersResultDTO>> Handle(
      GetUsersByLastNameQuery query,
      CancellationToken cancellationToken)
    {
      var offset = query.PerPage * (query.Page - 1);

      var users = await _userRepository.GetManyByLastNameAsync(query.LastName, offset, query.PerPage);

      if (users is null)
      {
        return CustomErrors.Database.FetchError;
      }

      var count = await _userRepository.CountByLastNameAsync(query.LastName);

      return new UsersResultDTO(
        Messages.Database.FetchSuccess,
        count,
        users);
    }
  }
}
