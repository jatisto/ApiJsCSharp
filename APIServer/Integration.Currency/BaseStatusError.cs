using System.Collections.Generic;
using System.Xml.Serialization;

namespace Integration.Currency
{
    public class BaseStatusError
    {
        [XmlIgnore]
        public List<string> ErrorMessage { get; set; }
    }
}