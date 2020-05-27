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
        public ActionResult<List<Server>> Get()
        {
            List<Server> syncServers = sql.ServerList();
            if (syncServers.Count == 0)
            {
                return NotFound();
            }
            return Ok(syncServers);
        }

      /*  // GET: api/Servers/5
        [HttpGet("{id}", Name = "GetServer")]
        public string GetServer(int id)
        {

            return "value";
        }*/

        // POST: api/Servers
        [HttpPost]
        public ActionResult Post([FromBody] Server server)
        {
            try
            {
                sql.addServer(server);
                return Created("new server added to the data base",server);
            }
            catch
            {
                return StatusCode(500);
            }
           
        }


        // DELETE: api/Servers/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                //remove it from db
                sql.deleteServer(id);
                return Ok();
            }
            catch
            {
                return NotFound(id);
            }
           
        }
    }
}
