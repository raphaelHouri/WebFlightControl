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
    public class FlightPlanController : ControllerBase
    {
        //SQL part
        private SQLCommands sql = new SQLCommands();

        // GET: api/FlightPlan/5
        [HttpGet("{id}", Name = "GetFlightPlan")]
        public FlightPlan GetFlightPlan(string id)
        {
            //find the flighplan in db
            FlightPlan flightPlan= sql.flightsplanById(id).GetFlightPlan();
            return flightPlan;
        }

        // POST: api/FlightPlan
        [HttpPost]
        public FlightPlan Post([FromBody] FlightPlan p)
        {
            // p.Id = 5;
            // return CreatedAtAction(actionName: "GetItem", new { id = p.Id }, p);

            sql.addPlan(p);
            return p;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
