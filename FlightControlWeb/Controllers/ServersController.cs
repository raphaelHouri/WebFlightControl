using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        //list of the servers the server sync with
        private List<Server> syncServers = new List<Server>();
        // GET: api/Servers
        [HttpGet]
        public IEnumerable<Server> Get()
        {
            return syncServers;
        }

      /*  // GET: api/Servers/5
        [HttpGet("{id}", Name = "GetServer")]
        public string GetServer(int id)
        {

            return "value";
        }*/

        // POST: api/Servers
        [HttpPost]
        public void Post([FromBody] Server server)
        {
            this.syncServers.Add(server);
        }


        // DELETE: api/Servers/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            //remove it from db
        }
    }
}
