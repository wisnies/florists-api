using Florists.Application.Features.ProductTransactions.Queries.GetProductTransactions;
using Florists.Core.Contracts.ProductTransactions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Florists.API.Controllers.v1
{
  [Route("api/v{version:apiVersion}/productTransactions")]
  [ApiVersion("1.0")]
  [Authorize]
  public class ProductTransactionsController : ApiController
  {
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public ProductTransactionsController(IMapper mapper, ISender mediator)
    {
      _mapper = mapper;
      _mediator = mediator;
    }

    [HttpGet("transactions")]
    public async Task<IActionResult> GetProductTransactions([FromQuery] GetProductTransactionsRequest request)
    {
      var query = _mapper.Map<GetProductTransactionsQuery>(request);

      var result = await _mediator.Send(query);

      return result.Match(
        result => Ok(_mapper.Map<ProductTransactionsResponse>(result)),
        errors => Problem(errors));
    }
  }
}
