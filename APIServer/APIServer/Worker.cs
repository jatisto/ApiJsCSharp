using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Integration.Currency;
using Integration.Currency.Response;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace APIServer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private RequestManager _manager;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                HttpSettings settings = new HttpSettings()
                {
                    FileXml = "daily.xml"
                };
                
                _manager = new RequestManager(settings);
                
                var result = await _manager.GetCurrency();
                Console.WriteLine($"Name: {result.Name}, Date: {result.Date}");
                foreach (var item in result.Currencies)
                {
                    Console.WriteLine("-----------------------/n");
                    Console.WriteLine(item.ISOCode);
                    Console.WriteLine(item.Nominal);
                    Console.WriteLine(Convert.ToDecimal(item.Value));
                    Console.Write("-----------------------");
                }
                await Task.Delay(60*1000, stoppingToken);
            }
        }
    }
}