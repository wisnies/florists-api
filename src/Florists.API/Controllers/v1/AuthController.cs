using Florists.Application.Features.Auth.Commands.ChangePassword;
using Florists.Application.Features.Auth.Commands.Login;
using Florists.Application.Features.Auth.Commands.Logout;
using Florists.Application.Features.Auth.Commands.RefreshToken;
using Florists.Core.Contracts.Auth;
using Florists.Core.Contracts.Common;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Florists.API.Controllers.v1
{
  [Route("api/v{version:apiVersion}/auth")]
  [ApiVersion("1.0")]
  [Authorize]
  public class AuthController : ApiController
  {
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthController(
      ISender mediator,
      IMapper mapper)
    {
      _mediator = mediator;
      _mapper = mapper;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
      var command = _mapper.Map<LoginCommand>(request);

      var result = await _mediator.Send(command);

      return result.Match(
        result => Ok(
          _mapper.Map<AuthResponse>(result)),
        errors => Problem(errors)
        );
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
      var identity = (ClaimsIdentity?)HttpContext.User.Identity;
      var email = GetEmailFromUserClaims(identity);

      var command = new LogoutCommand(email);

      var result = await _mediator.Send(command);

      return result.Match(
        result => Ok(
          _mapper.Map<MessageResponse>(result)),
        errors => Problem(errors)
        );
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
      var command = _mapper.Map<RefreshTokenCommand>(request);

      var result = await _mediator.Send(command);

      return result.Match(
        result => Ok(
          _mapper.Map<AuthResponse>(result)),
        errors => Problem(errors)
        );
    }

    [HttpPost("changePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
      var identity = (ClaimsIdentity?)HttpContext.User.Identity;
      var email = GetEmailFromUserClaims(identity);

      var command = _mapper.Map<ChangePasswordCommand>((request, email));

      var result = await _mediator.Send(command);

      return result.Match(
        result => Ok(
          _mapper.Map<MessageResponse>(result)),
        errors => Problem(errors)
        );
    }

    private string GetEmailFromUserClaims(ClaimsIdentity? identity)
    {
      if (identity is null)
      {
        return string.Empty;
      }

      var email = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

      if (email is null)
      {
        return string.Empty;
      }

      return email.Value;
    }
  }
}
