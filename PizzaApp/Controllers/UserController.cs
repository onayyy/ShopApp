using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models.ProductRequest;
using ShopAPI.Models.UserRequest;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("{register}")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request,[FromRoute] string register)
        {
            var result = await _mediator.Send(request.ToCommand(register));
            return Ok(result);
        }

        [HttpPost("{id}/address")]
        public async Task<IActionResult> CreateAddressWithUserById([FromBody] CreateAddressWithUserByIdRequest request, [FromRoute] int id)
        {
            var result = await _mediator.Send(request.ToCommand(id));
            return Ok(result);
        }
    }
}
