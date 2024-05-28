using Application.Address.Commands;
using Application.Address.Queries;
using Application.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models.AddressRequest;
using ShopAPI.Models.UserRequest;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddress()
        {
            var query = new GetAddressQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var query = new GetAddressByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var result = await _mediator.Send(new DeleteAddressCommand(id));
            return Ok("Adres Başarıyla Silindi.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressRequest request, [FromRoute] int id)
        {
            var result = await _mediator.Send(request.ToCommand(id));
            return Ok("Adres Başarıyla Güncellendi");
        }
    }
}
