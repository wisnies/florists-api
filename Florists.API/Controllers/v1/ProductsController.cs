using Florists.Application.Features.Products.Commands.CreateProduct;
using Florists.Core.Contracts.Products;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Florists.API.Controllers.v1
{
  [Route("api/v{version:apiVersion}/products")]
  [ApiVersion("1.0")]
  public class ProductsController : ApiController
  {
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public ProductsController(
      IMapper mapper,
      ISender mediator)
    {
      _mapper = mapper;
      _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBouqet(CreateProductRequest request)
    {
      var command = _mapper.Map<CreateProductCommand>(request);

      var result = await _mediator.Send(command);

      return result.Match(result => Ok(_mapper.Map<ProductResponse>(result)),
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
