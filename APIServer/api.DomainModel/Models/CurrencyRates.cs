using System.Collections.Generic;

namespace api.DomainContext.Models
{
    public class CurrencyRates
    {
        public string Name { get; set; }
      
        public string Date { get; set; }

        public List<Currency> Currencies { get; set; }
    }
}