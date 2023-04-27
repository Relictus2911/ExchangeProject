using ExchangeProject.Database;
using ExchangeProject.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeProject.UpdateDatabase
{
    internal class Program
    {
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ExchangeProjectContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ExchangeProject;Trusted_Connection=True;MultipleActiveResultSets=true");
            ExchangeProjectContext exchangeRateContext = new ExchangeProjectContext(optionsBuilder.Options);
            try
            {
                using HttpResponseMessage response = await client.GetAsync("https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt?year=" + DateTime.Now.Year);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var lines = responseBody.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                UpdateDatabase(lines, exchangeRateContext);

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
        public static void UpdateDatabase(string[] lines, ExchangeProjectContext context)
        {
            var headers = lines[0].Split("|");
            List<Currency> currencyList = new List<Currency>();

            var existingCurrencies = context.Currencies.ToList();
            var existingExchganeDates = context.ExchangeDates.ToList();

            for (int i = 1; i < headers.Length; i++)
            {
                var header = headers[i].Split(" ");
                var currency = new Currency();
                currency.Amount = int.Parse(header[0]);
                currency.Code = header[1];

                currencyList.Add(existingCurrencies.Find(a => a.Code == currency.Code) ?? currency);
            }

            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            for (int i = 1; i < lines.Length; i++)
            {

                var rowValues = lines[i].Split("|");
                if (rowValues.Contains("Date"))
                {

                    if (exchangeRates.Any())
                    {
                        context.ExchangeRates.AddRange(exchangeRates);
                        context.SaveChanges();
                    }
                    UpdateDatabase(lines.Take(new Range(i, lines.Length - 1)).ToArray(), context);
                    return;
                }
                var exchangeDate = new ExchangeDate(rowValues[0]);

                for (int j = 1; j < rowValues.Length; j++)
                {
                    if (existingExchganeDates.Find(a => a.Date == exchangeDate.Date) == null || existingCurrencies.Find(a=> a.Code == currencyList[j - 1].Code) == null)
                    {
                        var exchangeRate = new ExchangeRate();
                        exchangeRate.ExchangeDate = existingExchganeDates.Find(a => a.Date == exchangeDate.Date) ?? exchangeDate;
                        exchangeRate.Rate = decimal.Parse(rowValues[j].Replace('.', ','));
                        exchangeRate.Currency = currencyList[j - 1];
                        exchangeRates.Add(exchangeRate);
                    }
                }

            }

            //var newCurrenciesCode = currencyList.Select(item => item.Code).Except(existingCurrencies.Select(x => x.Code));
            //var newExchangeDates = exchangeRates.Select(item => item.ExchangeDate.Date).Except(existingExchganeDates.Select(x => x.Date));

            ////add new entities only if we have new dates or currencies
            //var newExchangeRates = exchangeRates
            //    .Where(item => newCurrenciesCode.Contains(item.Currency.Code) || newExchangeDates.Contains(item.ExchangeDate.Date));
            if (exchangeRates.Any())
            {
                context.ExchangeRates.AddRange(exchangeRates);
                context.SaveChanges();
            }
        }

        public static void SaveChanges(List<ExchangeRate> exchangeRates, ExchangeProjectContext context)
        {

        }

    }
}