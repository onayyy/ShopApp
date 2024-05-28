using Application.Common.Interfaces.Tokens;
using Application.User.Commands;
using Application.User.Queries;
using MediatR;
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
        private readonly ITokenService _tokenService;

        public UserController(IMediator mediator, ITokenService tokenService)
        {
            _mediator = mediator;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var query = new GetUserQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("register")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var result = await _mediator.Send(request.ToCommand());
            return Ok(result);
        }

        [HttpPost("{id}/address")]
        public async Task<IActionResult> CreateAddressWithUserById([FromBody] CreateAddressWithUserByIdRequest request, [FromRoute] int id)
        {
            var result = await _mediator.Send(request.ToCommand(id));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            return Ok("Kullanıcı başarılıyla silindi.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request, [FromRoute] int id)
        {
            var result = await _mediator.Send(request.ToCommand(id));
            return Ok("Kullanıcı Başarıyla Güncellendi.");
        }
    }
}
