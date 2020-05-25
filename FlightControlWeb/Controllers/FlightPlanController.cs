using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {

        // GET: api/FlightPlan/5
        [HttpGet("{id}", Name = "GetFlightPlan")]
        public FlightPlan GetFlightPlan(string id)
        {
            //find the flighplan in db
            FlightPlan f=null;
            return f;
        }

        // POST: api/FlightPlan
        [HttpPost]
        public FlightPlan Post([FromBody] FlightPlan p)
        {
            // p.Id = 5;
            // return CreatedAtAction(actionName: "GetItem", new { id = p.Id }, p);

            //SQL part
            Database databaseObject = new Database();
            SQLCommands sql = new SQLCommands();
            sql.addPlan(p, databaseObject);
            return p;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
