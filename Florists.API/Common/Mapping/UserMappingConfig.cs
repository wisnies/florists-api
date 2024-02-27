using Florists.Application.Features.User.Commands.EditUser;
using Florists.Core.Contracts.User;
using Mapster;

namespace Florists.API.Common.Mapping
{
  public class UserMappingConfig : IRegister
  {
    public void Register(TypeAdapterConfig config)
    {
      config.NewConfig<(EditUserRequest request, Guid userId), EditUserCommand>()
        .Map(dest => dest.UserId, src => src.userId)
        .Map(dest => dest, src => src.request);
    }
  }
}
