using ErrorOr;
using Florists.Application.Interfaces.Persistence;
using Florists.Core.Common.CustomErrors;
using Florists.Core.Common.Messages;
using Florists.Core.DTO.User;
using MediatR;

namespace Florists.Application.Features.User.Queries.GetUserById
{
  public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ErrorOr<UserResultDTO>>
  {
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<ErrorOr<UserResultDTO>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
      var user = await _userRepository.GetOneByIdAsync(query.UserId);

      if (user is null)
      {
        return CustomErrors.User.NotFound;
      }

      return new UserResultDTO(Messages.User.FetchSuccess, user);
    }
  }
}
