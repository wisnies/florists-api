using Florists.Application.Features.Inventories.Commands.CreateInventory;
using Florists.Application.Features.Inventories.Commands.EditInventory;
using Florists.Application.Features.Inventories.Commands.PurchaseInventories;
using Florists.Application.Features.Inventories.Queries.GetInventoriesByName;
using Florists.Application.Features.Inventories.Queries.GetInventoryById;
using Florists.Core.Contracts.Common;
using Florists.Core.Contracts.Inventories;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Florists.API.Controllers.v1
{
  [Route("api/v{version:apiVersion}/inventories")]
  [ApiVersion("1.0")]
  [Authorize]
  public class InventoriesController : ApiController
  {
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public InventoriesController(
      ISender mediator,
      IMapper mapper)
    {
      _mediator = mediator;
      _mapper = mapper;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateInventory(CreateInventoryRequest request)
    {
      var command = _mapper.Map<CreateInventoryCommand>(request);

      var result = await _mediator.Send(command);

      return result.Match(
        result => Ok(_mapper.Map<MessageResponse>(result)),
        errors => Problem(errors));
    }

    [HttpPost("edit/{inventoryId}")]
    public async Task<IActionResult> EditInventory(EditInventoryRequest request, Guid inventoryId)
    {
      var command = _mapper.Map<EditInventoryCommand>((request, inventoryId));

      var result = await _mediator.Send(command);

      return result.Match(
        result => Ok(_mapper.Map<MessageResponse>(result)),
        errors => Problem(errors));
    }

    [HttpGet("inventory/{inventoryId}")]
    public async Task<IActionResult> GetInventoryById(Guid inventoryId)
    {
      var query = new GetInventoryByIdQuery(inventoryId);

      var result = await _mediator.Send(query);

      return result.Match(
        result => Ok(_mapper.Map<InventoryResponse>(result)),
        errors => Problem(errors));
    }

    [HttpGet("inventories")]
    public async Task<IActionResult> GetInventoriesByName([FromQuery] GetInventoriesByNameRequest request)
    {
      var query = _mapper.Map<GetInventoriesByNameQuery>(request);

      var result = await _mediator.Send(query);

      return result.Match(
        result => Ok(_mapper.Map<InventoriesResponse>(result)),
        errors => Problem(errors));
    }


    [HttpPost("purchase")]
    public async Task<IActionResult> PurchaseInventories(PurchaseInventoriesRequest request)
    {
      var identity = (ClaimsIdentity?)HttpContext.User.Identity;
      var email = GetEmailFromUserClaims(identity);

      var command = _mapper.Map<PurchaseInventoriesCommand>((request, email));

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
