using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using api.DomainContext.Models;
using api.DomainContext.MongoDB;
using api.DomainContext.Settings;
using Integration.Currency;
using Integration.Currency.Response;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace APIServer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private RequestManager _manager;
        private MongoDbCRUD _mongoDb;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _mongoDb = new MongoDbCRUD(GetMongoSettingsDb());
            _manager = new RequestManager(GetHttpSettings());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var resultDb = GetTableFromDb<CurrencyRatesResponse>("CurrencyRateDaily");
                
                var resultXml = await _manager.GetCurrency();

                if (resultDb != null)
                {
                    var currenciesList = new List<Currency>();

                    foreach (var item in resultXml.Currencies)
                    {
                        currenciesList.Add(new Currency()
                        {
                            ISOCode = item.ISOCode,
                            Nominal = item.Nominal,
                            Value = item.Value
                        });
                    }

                    _mongoDb.Update("CurrencyRateDaily", "Currencies", currenciesList, resultDb.Id);
                }
                else
                {
                    _mongoDb.Insert("CurrencyRateDaily", resultXml);
                }

                _logger.LogInformation($"RateId: {resultDb.Id.ToString()}, Date: {resultDb.Date}");
                
                foreach (var item in resultDb.Currencies)
                {
                    _logger.LogInformation($"Валюта: {item.ISOCode}, Курс: {item.Value}");
                }
              
                await Task.Delay(60*1000, stoppingToken);
            }
        }

        private T GetTableFromDb<T>(string table)
        {
            return _mongoDb.FindDocs<T>(table).GetAwaiter().GetResult();
        }

        private HttpSettings GetHttpSettings() => new HttpSettings()
        {
            FileXml = "daily.xml"
        };

        private MongoSettingsDb GetMongoSettingsDb() => new MongoSettingsDb()
        {
            ConnectionString = "mongodb+srv://TM_User_1:ARK9hclBaReZv10R@clusterapi0.glu7e.mongodb.net/test",
            Database = "apiMongoDbCurrency"
        };
        
    }
}