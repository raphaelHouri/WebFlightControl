using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class ExternalFlights
    {
        public static Dictionary<string, string> DicFlightServer;
        public  async Task<FlightPlan> GetExternalFlightById(string id)
        {
            FlightPlan flightPlan = null;
            // List<FlightPlan> ex = new List<FlightPlan>();
            //Get
            //api/FlightPlan/" + id;
            SQLCommands commands = new SQLCommands();
            //find the server of the flight id
            string serverId = DicFlightServer[id];
            if (serverId == null)
            {
                return null;
            }
            else
            {
                Server server = commands.serverById(serverId);
                //like ther= rest of the code


                //  List<Server> ourServers = commands.ServerList();
                //foreach (Server server in ourServers)
                // {
                string idFlight = id;
                string urlServer = server.ServerUrl;
                string serverApi = urlServer + "/api/FlightPlan/" + idFlight;
                //string strurltest = String.Format("https://jsonplaceholder.typicode.com/posts/1/comments");
                string strurltest = String.Format(serverApi);
                WebRequest requestObjGet = WebRequest.Create(strurltest);
                requestObjGet.Method = "GET";
                WebResponse responseObjGet = null;
                responseObjGet = await requestObjGet.GetResponseAsync();
                string strresulttest = null;
                using (Stream stream = responseObjGet.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream);
                    strresulttest = sr.ReadToEnd();
                    sr.Close();
                }
                flightPlan = JsonConvert.DeserializeObject<FlightPlan>(strresulttest);
            }
        //    }

            return flightPlan;
        }
        public async Task<List<Flight>> GetExternalFlights(string time)
        {
            DicFlightServer = new Dictionary<string, string>();
            List<Flight> ex = null;
            // List<FlightPlan> ex = new List<FlightPlan>();
            //Get
            //api/FlightPlan/" + id;
            SQLCommands commands = new SQLCommands();
            List<Server> ourServers = commands.ServerList();
            foreach (Server server in ourServers)
            {
                string date_time = time;
                string urlServer = server.ServerUrl;
                string serverApi = urlServer + "/api/Flights?relative_to=" + date_time;
                //string strurltest = String.Format("https://jsonplaceholder.typicode.com/posts/1/comments");
                string strurltest = String.Format(serverApi);
                WebRequest requestObjGet = WebRequest.Create(strurltest);
                requestObjGet.Method = "GET";
                WebResponse responseObjGet = null;
                responseObjGet = await requestObjGet.GetResponseAsync();
                string strresulttest = null;
                using (Stream stream = responseObjGet.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream);
                    strresulttest = sr.ReadToEnd();
                    sr.Close();
                }
                ex = JsonConvert.DeserializeObject<List<Flight>>(strresulttest);
                //add mapping from each id flight to the server id
                foreach (Flight item in ex)
                {
                    DicFlightServer.Add(item.Flight_id, server.ServerId);
                }
            }

            return ex;
        }

        public List<Flight> changeBoolEX(List<Flight> flights)
        {
            foreach (Flight item in flights)
            {
                item.Is_external = true;
            }
            return flights;
        }
        /*   public async Task<string> try1()
               {
               WebResponse responseObjGet = null;
               responseObjGet = await requestObjGet.GetResponseAsync();

            HttpWebResponse responseObjGet = null;
                   responseObjGet = await (HttpWebResponse)requestObjGet.GetResponse();
           }*/

    }
}
