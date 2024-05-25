using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetExchangeRates()
        {
            var exchangeRates = await _currencyService.GetExchangeRatesAsync();

            if (exchangeRates != null)
            {
                return Ok(exchangeRates);
            }
            else
            {
                return StatusCode(500, "Error retrieving exchange rates.");
            }
        }
    }
}
