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

        private string serverId;
        private string serverUrl;

        [JsonConstructor]
        public Server(string serverId,String serverUrl)
        {
            this.serverId = serverId;
            this.serverUrl = serverUrl;
        }

        [JsonProperty("serverId")]
        public string ServerId { get; set; }
        
        [JsonProperty("serverUrl")]
        public string ServerUrl { get; set; }
    }

}
