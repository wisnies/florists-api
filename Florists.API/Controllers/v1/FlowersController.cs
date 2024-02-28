using Florists.Application.Features.Flowers.Commands.CreateFlower;
using Florists.Application.Features.Flowers.Commands.EditFlower;
using Florists.Application.Features.Flowers.Commands.PurchaseFlowers;
using Florists.Application.Features.Flowers.Queries.GetFlowerById;
using Florists.Application.Features.Flowers.Queries.GetFlowersByName;
using Florists.Core.Contracts.Common;
using Florists.Core.Contracts.Flowers;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Florists.API.Controllers.v1
{
  [Route("api/v{version:apiVersion}/flowers")]
  [ApiVersion("1.0")]
  [Authorize]
  public class FlowersController : ApiController
  {
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public FlowersController(
      ISender mediator,
      IMapper mapper)
    {
      _mediator = mediator;
      _mapper = mapper;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateFlower(CreateFlowerRequest request)
    {
      var command = _mapper.Map<CreateFlowerCommand>(request);

      var result = await _mediator.Send(command);

      return result.Match(
        result => Ok(_mapper.Map<MessageResponse>(result)),
        errors => Problem(errors));
    }

    [HttpPost("edit/{flowerId}")]
    public async Task<IActionResult> EditFlower(EditFlowerRequest request, Guid flowerId)
    {
      var command = _mapper.Map<EditFlowerCommand>((request, flowerId));

      var result = await _mediator.Send(command);

      return result.Match(
        result => Ok(_mapper.Map<MessageResponse>(result)),
        errors => Problem(errors));
    }

    [HttpGet("flower/{flowerId}")]
    public async Task<IActionResult> GetFlowerById(Guid flowerId)
    {
      var query = new GetFlowerByIdQuery(flowerId);

      var result = await _mediator.Send(query);

      return result.Match(
        result => Ok(_mapper.Map<FlowerResponse>(result)),
        errors => Problem(errors));
    }

    [HttpGet("flowers")]
    public async Task<IActionResult> GetFlowersByName([FromQuery] GetFlowersByNameRequest request)
    {
      var query = _mapper.Map<GetFlowersByNameQuery>(request);

      var result = await _mediator.Send(query);

      return result.Match(
        result => Ok(_mapper.Map<FlowersResponse>(result)),
        errors => Problem(errors));
    }


    [HttpPost("purchase")]
    public async Task<IActionResult> PurchaseFlowers(PurchaseFlowersRequest request)
    {
      var identity = (ClaimsIdentity?)HttpContext.User.Identity;
      var email = GetEmailFromUserClaims(identity);

      var command = _mapper.Map<PurchaseFlowersCommand>((request, email));

      var result = await _mediator.Send(command);

      return result.Match(
        result => Ok(_mapper.Map<MessageResponse>(result)),
        errors => Problem(errors));
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
