using System.Collections.Generic;
using System.Xml.Serialization;

namespace Integration.Currency.Response
{

    [XmlRoot("CurrencyRates")]
    public class CurrencyRates : BaseStatusError
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        [XmlElement(ElementName = "Currency")]
        public List<Currency> Currencies { get; set; }
    }

    public class Currency
    {
        [XmlAttribute(AttributeName = "ISOCode")]
        public string ISOCode { get; set; }

        public int Nominal { get; set; }

        public string Value { get; set; }
    }
}