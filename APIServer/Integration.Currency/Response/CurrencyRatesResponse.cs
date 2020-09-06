﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Integration.Currency.Response
{

    [XmlRoot("CurrencyRates")]
    public class CurrencyRatesResponse : BaseStatusError
    {
        [BsonId]
        [XmlIgnore]
        public ObjectId Id { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        [XmlElement(ElementName = "Currency")]
        public List<CurrencyResponse> Currencies { get; set; }
    }

    public class CurrencyResponse
    {
        [XmlAttribute(AttributeName = "ISOCode")]
        public string ISOCode { get; set; }

        public int Nominal { get; set; }
        public string Value { get; set; }
    }
}