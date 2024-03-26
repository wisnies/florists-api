using Florists.Application.Features.Users.Commands.EditUser;
using Florists.Application.Features.Users.Queries.GetUsersByLastName;
using Florists.Core.Contracts.User;
using Florists.Core.DTO.User;
using Florists.Core.Entities;
using Florists.Core.Enums;
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

      config.NewConfig<FloristsUser, UserDTO>()
        .Map(dest => dest, src => src)
        .Map(dest => dest.RoleType, src => src.Role != null ? src.Role.RoleType : RoleTypeOptions.Demo);

      config.NewConfig<List<FloristsUser>, List<UserDTO>>();

      config.NewConfig<UserResultDTO, UserResponse>()
        .Map(dest => dest.User, src => src.User)
        .Map(dest => dest.User.RoleType, src => src.User.Role != null ? src.User.Role.RoleType : RoleTypeOptions.Demo);

      config.NewConfig<GetUsersByLastNameRequest, GetUsersByLastNameQuery>()
        .Map(dest => dest.Page, src => src.Page <= 0 ? 1 : src.Page)
        .Map(dest => dest.PerPage, src => src.PerPage <= 0 ? (int)PerPageOptions.Ten : src.PerPage);
    }
  }
}
