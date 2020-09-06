using System.Collections.Generic;
using System.Xml.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Integration.Currency
{
    public class BaseStatusError
    {
        [XmlIgnore]
        [BsonIgnore]
        public List<string> ErrorMessage { get; set; }
    }
}