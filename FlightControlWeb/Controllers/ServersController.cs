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
        private SQLCommands sql = new SQLCommands();
        // GET: api/Servers
        [HttpGet]
        public IEnumerable<Server> Get()
        {
            List<Server> syncServers = sql.ServerList();
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
        public Server Post([FromBody] Server server)
        {
            sql.addServer(server);
            return server;
        }


        // DELETE: api/Servers/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            //remove it from db
            sql.deleteServer(id);
        }
    }
}
