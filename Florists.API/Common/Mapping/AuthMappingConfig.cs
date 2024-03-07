using Florists.Application.Features.Auth.Commands.ChangePassword;
using Florists.Core.Contracts.Auth;
using Florists.Core.DTO.Auth;
using Florists.Core.Enums;
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

      config.NewConfig<AuthResultDTO, AuthResponse>()
        .Map(dest => dest.User, src => src.User)
        .Map(dest => dest.User.RoleType, src => src.User.Role != null ? src.User.Role.RoleType : RoleTypeOptions.Demo)
        .Map(dest => dest.Tokens, src => src.Tokens);
    }
  }
}
