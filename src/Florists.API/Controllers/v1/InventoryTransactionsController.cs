using Florists.Application.Features.InventoryTransactions.Queries.GetInventoryTransactions;
using Florists.Core.Contracts.InventoryTransactions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Florists.API.Controllers.v1
{
  [Route("api/v{version:apiVersion}/inventoryTransactions")]
  [ApiVersion("1.0")]
  [Authorize]
  public class InventoryTransactionsController : ApiController
  {
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public InventoryTransactionsController(IMapper mapper, ISender mediator)
    {
      _mapper = mapper;
      _mediator = mediator;
    }

    [HttpGet("transactions")]
    public async Task<IActionResult> GetInventoryTransactions([FromQuery] GetInventoryTransactionsRequest request)
    {
      var query = _mapper.Map<GetInventoryTransactionsQuery>(request);

      var result = await _mediator.Send(query);

      return result.Match(
        result => Ok(_mapper.Map<InventoryTransactionsResponse>(result)),
        errors => Problem(errors));
    }
  }
}
