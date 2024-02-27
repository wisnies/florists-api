using Florists.Application.Features.Auth.Commands.ChangePassword;
using Florists.Core.Contracts.Auth;
using Mapster;

namespace Florists.API.Common.Mapping
{
  public class AuthMappingConfig : IRegister
  {
    public void Register(TypeAdapterConfig config)
    {
      config.NewConfig<(ChangePasswordRequest request, string email), ChangePasswordCommand>()
        .Map(dest => dest.Email, src => src.email)
        .Map(dest => dest, src => src.request);
    }
  }
}
