using System.Threading.Tasks;
using Integration.Currency;
using Integration.Currency.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiCurency.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyApiController : ControllerBase
    {
        private readonly ILogger<CurrencyApiController> _logger;

        public CurrencyApiController(ILogger<CurrencyApiController> logger)
        {
            _logger = logger;
        }

        // GET
        [HttpGet]
        public Task<CurrencyRatesResponse> Get()
        {
            var settings = new HttpSettings()
            {
                FileXml = "daily.xml"
            };
            var manager = new RequestManager(settings);
            return manager.GetCurrency();
        }
    }
}