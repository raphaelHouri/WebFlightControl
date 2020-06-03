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
        private readonly ISQLCommands sql;
        private readonly IExternalFlights externalFlights;
        public ServersController(ISQLCommands sql, IExternalFlights externalFlights)
        {
            this.sql = sql;
            this.externalFlights = externalFlights;
        } 
            
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

        // POST: api/Servers
        [HttpPost]
        public ActionResult Post([FromBody] Server server)
        {
            try
            {
                sql.AddServer(server);
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
                sql.DeleteServer(id);
                //and from dic
                externalFlights.DeleteDic(id);
                return Ok();
            }
            catch
            {
                return NotFound(id);
            }
           
        }
    }
}
