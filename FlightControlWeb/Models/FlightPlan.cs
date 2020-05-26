

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


        [JsonProperty("passengers")]
        public int Passengers { get; set; }

        [JsonProperty("company_name")]
        public string Company_Name { get; set; }

        [JsonProperty("initial_location")]
        public InitialLocation Initial_Location { get; set; }

        [JsonProperty("segments")]
        public List<Segment> Segments { get; set; }
        public FlightPlan( )
        {

        } 
        public FlightPlan( int passengers, string company_name, InitialLocation initial_location, List<Segment> segments)
        {
   
            this.Passengers = passengers;
            this.Company_Name = company_name;
            this.Initial_Location = initial_location;
            this.Segments = segments;
        }
    }

    public partial class InitialLocation
    {

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("date_time")]
        public DateTime Date_Time { get; set; }
        public InitialLocation() { }
        public InitialLocation(double lng, double lat, DateTime date_time)
        {
            this.Longitude = lng;
            this.Latitude = lat;
            this.Date_Time = date_time;
        }
    }

    public partial class Segment
    {
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("timespan_seconds")]
        public double Timespan_Seconds { get; set; }


        public Segment() { }
        public Segment(double lng,double lat,double time)
        {
            this.Longitude = lng;
            this.Latitude = lat;
            this.Timespan_Seconds = time;
        }
        
        
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



