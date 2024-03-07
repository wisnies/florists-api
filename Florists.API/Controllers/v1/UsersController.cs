using Florists.Application.Features.Users.Commands.CreateUser;
using Florists.Application.Features.Users.Commands.DeleteUser;
using Florists.Application.Features.Users.Commands.EditUser;
using Florists.Application.Features.Users.Queries.GetUserById;
using Florists.Application.Features.Users.Queries.GetUsersByLastName;
using Florists.Core.Contracts.User;
using Florists.Infrastructure.Policies;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Florists.API.Controllers.v1
{
  [Route("api/v{version:apiVersion}/users")]
  [ApiVersion("1.0")]
  [Authorize(Policy = ForAdminPolicy.Name)]
  public class UsersController : ApiController
  {
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public UsersController(
      ISender mediator,
      IMapper mapper)
    {
      _mediator = mediator;
      _mapper = mapper;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
      var command = _mapper.Map<CreateUserCommand>(request);

      var result = await _mediator.Send(command);

      return result.Match(result => Ok(
        _mapper.Map<UserResponse>(result)),
        errors => Problem(errors));
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
      var query = new GetUserByIdQuery(userId);

      var result = await _mediator.Send(query);

      return result.Match(result => Ok(
        _mapper.Map<UserResponse>(result)),
        errors => Problem(errors));
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsersByLastName([FromQuery] GetUsersByLastNameRequest request)
    {
      var query = _mapper.Map<GetUsersByLastNameQuery>(request);

      var result = await _mediator.Send(query);

      return result.Match(result => Ok(
        _mapper.Map<UsersResponse>(result)),
        errors => Problem(errors));
    }

    [HttpPost("edit/{userId}")]
    public async Task<IActionResult> EditUser(EditUserRequest request, Guid userId)
    {
      var command = _mapper.Map<EditUserCommand>((request, userId));

      var result = await _mediator.Send(command);

      return result.Match(result => Ok(
        _mapper.Map<UserResponse>(result)),
        errors => Problem(errors));
    }

    [HttpDelete("delete/{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
      var command = new DeleteUserCommand(userId);

      var result = await _mediator.Send(command);

      return result.Match(result => Ok(
        _mapper.Map<UserResponse>(result)),
        errors => Problem(errors));
    }
  }
}
