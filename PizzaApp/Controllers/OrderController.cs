using Application.Order.Commands;
using Application.Order.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models.OrderRequest;
using ShopAPI.Models.ProductRequest;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var query = new GetOrderQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var query = new GetOrderByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("save-order")]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var result = await _mediator.Send(request.ToCommand());
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            var result = await _mediator.Send(new DeleteOrderCommand(id));
            return Ok("Sipariş Başarıyla Silindi");
        }
    }
}

