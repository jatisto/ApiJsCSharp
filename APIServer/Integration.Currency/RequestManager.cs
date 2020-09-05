using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Integration.Currency.Response;
using Microsoft.Extensions.Logging;

namespace Integration.Currency
{
    public class RequestManager
    {
        private readonly BaseApiClient _baseApiClient;
        public RequestManager(HttpSettings settings)
        {
            _baseApiClient = new BaseApiClient(settings);
        }

        public async Task<CurrencyRatesResponse> GetCurrency() =>
            await Execute(async () => await _baseApiClient.ExecuteRequest<CurrencyRatesResponse>());
            
        
        private static async Task<TResponse> Execute<TResponse>(Func<Task<TResponse>> request) where TResponse : BaseStatusError, new()
        {
            ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, error) => true;

            try
            {
                return await request();
            }
            catch (Exception e)
            {
                return new TResponse()
                {
                    ErrorMessage = new List<string> { e.Message }
                }; 
                    
            }
        }
        
        public static T XmlToObject<T>(string xml)
        {
            using (var xmlStream = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(XmlReader.Create(xmlStream));
            }
        }

    }
}