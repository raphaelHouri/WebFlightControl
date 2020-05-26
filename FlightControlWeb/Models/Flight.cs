using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Flight
    {
     
        [JsonProperty("flight_id")]
        [JsonPropertyName("flight_id")]
        public string Flight_id { get; set; } 
        
        
        [JsonProperty("longitude")]
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; } 
        
        [JsonProperty("latitude")]
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }


        [JsonProperty("passengers")]
        [JsonPropertyName("passengers")]
        public long Passengers { get; set; }
        

        [JsonProperty("company_name")]
        [JsonPropertyName("company_name")]
        public string Company_name { get; set; }  
        
        
        [JsonProperty("date_time")]
        [JsonPropertyName("date_time")]
        public DateTime Date_time { get; set; }
        
        [JsonProperty("is_external")]
        [JsonPropertyName("is_external")]
        public bool Is_external { get; set; }


        [JsonConstructor]
        public Flight(string flight_id, double longitude, double latitude,
            long passengers, string company_name, DateTime date_time, bool is_external)
        {
            this.Flight_id = flight_id;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Passengers = passengers;
            this.Company_name = company_name;
            this.Date_time = date_time;
            this.Is_external = is_external;
        }

    }

}
