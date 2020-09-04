using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Integration.Currency.Response;

namespace Integration.Currency
{
    public class BaseApiClient
    {
        public readonly HttpSettings Settings;
        
        private const string RequestUri = "https://www.nbkr.kg/XML/";
        public BaseApiClient(HttpSettings settings)
        {
            Settings = settings;
        }
        
        public async Task<TResponse> ExecuteRequest<TResponse>()
        {
            using (var client = GetHttpClient())
            {
                var serializer = new XmlSerializer(typeof(TResponse));
                CurrencyRates obj;
                
                using (Stream s = await client.GetStreamAsync(RequestUri + Settings.FileXml))
                {
                    using (StreamReader str = new StreamReader(s))
                    {
                        return (TResponse) serializer.Deserialize(str);
                    }
                    
                }
            }
        }

        private HttpClient GetHttpClient()
        {
            var handler = new HttpClientHandler();
            
            return new HttpClient(handler)
            {
                DefaultRequestHeaders =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Xml) }
                }
            };
        }
    }
}