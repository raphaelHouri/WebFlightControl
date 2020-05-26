using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Server
    {

        [JsonProperty("ServerId")]
        public string ServerId { get; set; }

        [JsonProperty("ServerURL")]
        public string ServerUrl { get; set; }
       
        [JsonConstructor]
        public Server(string serverId,String serverUrl)
        {
            this.ServerId = serverId;
            this.ServerUrl = serverUrl;
        }

      
    }

}
