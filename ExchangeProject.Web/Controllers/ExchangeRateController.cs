using ExchangeProject.Database;
using ExchangeProject.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExchangeProject.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        ExchangeProjectContext exchangeRateContext;
        public ExchangeRateController (ExchangeProjectContext exchangeRateContext)
        {
            this.exchangeRateContext = exchangeRateContext;
        }

        [HttpGet]
        public IEnumerable<Currency> Get()
        {
            return exchangeRateContext.Currencies.ToList();
        }

        [HttpGet("{code}/{date}")]
        public ExchangeRate GetExchangeRates(string code, DateTime date)
        {
            var closestPreviousDate = exchangeRateContext.ExchangeDates.Where(x => x.Date <= date).OrderByDescending(x => x.Date).FirstOrDefault();

            return exchangeRateContext.ExchangeRates.Where(item => item.Currency.Code == code && item.ExchangeDate.Date == closestPreviousDate.Date)
                .Include(a => a.Currency).FirstOrDefault();
        }

        [HttpGet("firstdate")]
        public DateTime GetFirstExisitingDate()
        {
            return exchangeRateContext.ExchangeDates.OrderBy(x => x.Date).FirstOrDefault().Date;
        }

        [HttpGet("lastdate")]
        public DateTime GetLastExisitingDate()
        {
            return exchangeRateContext.ExchangeDates.OrderByDescending(x => x.Date).FirstOrDefault().Date;
        }
    }
}
