using Florists.Application.Features.Products.Commands.CreateProduct;
using Florists.Application.Features.Products.Commands.DeleteProduct;
using Florists.Application.Features.Products.Commands.EditProduct;
using Florists.Application.Features.Products.Commands.ProduceProduct;
using Florists.Application.Features.Products.Commands.SellProducts;
using Florists.Application.Features.Products.Queries.GetProductById;
using Florists.Application.Features.Products.Queries.GetProductsByName;
using Florists.Core.Contracts.Common;
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
    public async Task<IActionResult> CreateProduct(CreateProductRequest request)
    {
      var command = _mapper.Map<CreateProductCommand>(request);

      var result = await _mediator.Send(command);

      return result.Match(result => Ok(_mapper.Map<ProductResponse>(result)),
        errors => Problem(errors));
    }

    [HttpPost("edit/{productId}")]
    public async Task<IActionResult> EditProduct(EditProductRequest request, Guid productId)
    {
      var command = _mapper.Map<EditProductCommand>((request, productId));

      var result = await _mediator.Send(command);

      return result.Match(result => Ok(_mapper.Map<MessageResponse>(result)),
        errors => Problem(errors));
    }

    [HttpDelete("delete/{productId}")]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
      var command = new DeleteProductCommand(productId);

      var result = await _mediator.Send(command);

      return result.Match(result => Ok(_mapper.Map<ProductResponse>(result)),
        errors => Problem(errors));
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProductsByName([FromQuery] GetProductsByNameRequest request)
    {
      var query = _mapper.Map<GetProductsByNameQuery>(request);

      var result = await _mediator.Send(query);

      return result.Match(result => Ok(_mapper.Map<ProductsResponse>(result)),
        errors => Problem(errors));
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetProductById(Guid productId)
    {
      var query = new GetProductByIdQuery(productId);

      var result = await _mediator.Send(query);

      return result.Match(result => Ok(_mapper.Map<ProductResponse>(result)),
        errors => Problem(errors));
    }

    [HttpPost("produce")]
    public async Task<IActionResult> ProduceProduct(ProduceProductRequest request)
    {
      var identity = (ClaimsIdentity?)HttpContext.User.Identity;
      var email = GetEmailFromUserClaims(identity);

      var command = _mapper.Map<ProduceProductCommand>((request, email));

      var result = await _mediator.Send(command);

      return result.Match(result => Ok(_mapper.Map<MessageResponse>(result)),
        errors => Problem(errors));
    }

    [HttpPost("sell")]
    public async Task<IActionResult> SellProducts(SellProductsRequest request)
    {
      var identity = (ClaimsIdentity?)HttpContext.User.Identity;
      var email = GetEmailFromUserClaims(identity);

      var command = _mapper.Map<SellProductsCommand>((request, email));

      var result = await _mediator.Send(command);

      return result.Match(result => Ok(_mapper.Map<MessageResponse>(result)),
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
