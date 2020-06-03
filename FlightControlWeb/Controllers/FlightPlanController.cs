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
        private readonly ISQLCommands sql;
        private readonly IExternalFlights externalFlights;
        public FlightPlanController(IExternalFlights externalFlights, ISQLCommands sql)
        {
            this.externalFlights = externalFlights;
            this.sql = sql;
        }

        // GET: api/FlightPlan/5
        [HttpGet("{id}", Name = "GetFlightPlan")]
        public async Task<ActionResult<FlightPlan>> GetFlightPlan(string id)
        {
            //find the flighplan in db
            FlightPlanDB flightPlanDB = sql.flightsplanById(id);
            FlightPlan flightPlan;
            if (flightPlanDB == null)
            {
                try
                {
                    flightPlan = await externalFlights.GetExternalFlightById(id);

                }
                catch
                {
                    return NotFound();
                }
            }
            else
            {
                flightPlan = flightPlanDB.GetFlightPlan();
            }

            if (flightPlan == null)
            {
                return NotFound();
            }
            return Ok(flightPlan);
            
        }

        // POST: api/FlightPlan
        [HttpPost]
        public ActionResult Post([FromBody] FlightPlan p)
        {
            try
            {
                sql.addPlan(p);
                return Created("new flight plan added to the data base",p);
            }
            catch
            {
                //Internal Server Error
                return StatusCode(500);
            }

        }

    }
}
