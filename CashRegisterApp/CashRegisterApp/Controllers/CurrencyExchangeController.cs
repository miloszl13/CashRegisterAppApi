using ApplicationLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CashRegisterApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyExchangeController : ControllerBase
    {
        private readonly ICurrencyExchangeService _currencyExchangeService;
        public CurrencyExchangeController(ICurrencyExchangeService currencyExchangeService)
        {
            _currencyExchangeService = currencyExchangeService;
        }
        [HttpGet("Exchange/{currency},{amount:int},{currency1}")]
        public ActionResult<double> Exchange([FromRoute] string currency, [FromRoute] int amount, [FromRoute] string currency1)
        {
            var result = _currencyExchangeService.Calculate(currency, amount, currency1);
            return result;
        }
    }
}
