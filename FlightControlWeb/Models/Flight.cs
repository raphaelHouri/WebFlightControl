using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Flight
    {
        private string flight_id;
        private double longitude;
        private double latitude;
        private long passengers;
        private string company_name;
        private DateTime date_time;
        private bool is_external;

        [JsonConstructor]
        public Flight(string flight_id,double longitude, double latitude, 
            long passengers, string company_name, DateTime date_time, bool is_external)
        {
            this.flight_id = flight_id;
            this.longitude = longitude;
            this.latitude = latitude;
            this.passengers = passengers;
            this.company_name = company_name;
            this.date_time = date_time;
            this.is_external = is_external;
        }


        [JsonProperty("flight_id")]
        public string Flight_id { get; set; } 
        
        
        [JsonProperty("longitude")]
        public double Longitude { get; set; } 
        
        [JsonProperty("latitude")]
        public double Latitude { get; set; }


        [JsonProperty("passengers")]
        public long Passengers { get; set; }
        

        [JsonProperty("company_name")]
        public string Company_name { get; set; }  
        
        
        [JsonProperty("date_time")]
        public DateTime Date_time { get; set; }
        
        [JsonProperty("is_external")]
        public string Is_external { get; set; }
       
    }
}
