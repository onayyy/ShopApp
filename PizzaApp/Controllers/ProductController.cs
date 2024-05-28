
using Application.Product.Commands;
using Application.Product.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models.ProductRequest;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var result = await _mediator.Send(request.ToCommand());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            var query = new GetProductQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));
            return Ok("Ürün başarılıyla silindi.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request, [FromRoute] int id)
        {
            var result = await _mediator.Send(request.ToCommand(id));
            return Ok("Ürün Başarıyla Güncellendi.");
        }
    }
}
