

namespace FlightControlWeb
{
    using FlightControlWeb;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class FlightPlan
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("passengers")]
        public long Passengers { get; set; }

        [JsonProperty("company_name")]
        public string Company_Name { get; set; }

        [JsonProperty("initial_location")]
        public InitialLocation Initial_Location { get; set; }

        [JsonProperty("segments")]
        public List<Segment> Segments { get; set; }
    }

    public partial class InitialLocation
    {
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("date_time")]
        public DateTime DateTime { get; set; }
    }

    public partial class Segment
    {
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitde")]
        public double Latitde { get; set; }

        [JsonProperty("timespan_seconds")]
        public double TimespanSeconds { get; set; }
    }

    public partial class Welcome
    {
        public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, FlightControlWeb.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Welcome self) => JsonConvert.SerializeObject(self, FlightControlWeb.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
    
}



