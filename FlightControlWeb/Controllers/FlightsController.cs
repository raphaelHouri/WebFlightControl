using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using System.Data.SQLite;

namespace FlightControlWeb.Controllers
{
   

    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private FlightCalculator calculator = new FlightCalculator();

        private readonly ISQLCommands sql;
        private readonly IExternalFlights externalFlights;
        public FlightsController(ISQLCommands sql,IExternalFlights externalFlights)
        {
            this.sql = sql;
            this.externalFlights = externalFlights;
        }

        //injection
        // GET: api/Flights?relative_to=<DATE_TIME>
        [HttpGet]
        public async Task<ActionResult<List<Flight>>> 
            GetAllFlights([FromQuery(Name = "relative_to")] string relative_to)
        {
            List<Flight> flights = new List<Flight>();
            bool checkSyncAll = Request.Query.ContainsKey("sync_all");
            if (checkSyncAll)
            {
                //need to get flights from out servers
                List<Flight> exFlights = await externalFlights.GetExternalFlights(relative_to);
                if(exFlights != null)
                {
                    exFlights = externalFlights.ChangeBoolEX(exFlights);
                    flights.AddRange(exFlights);
                }
            }
           
            //the list of flights we will send to the client to update the marker
            //we need to get from the db all the flight plan that are relvante
            List<FlightPlanDB> flightList = sql.FlightsList(relative_to);

            for (int i = 0; i < flightList.Count; i++)
            {
                string id = flightList[i].GetId();
                FlightPlan flightPlan = flightList[i].GetFlightPlan();

                //step 1. subset from  current datetime to initial(need to convert the string).
                double diff = calculator.SubTime(flightPlan.Initial_Location.Date_Time, relative_to);
                //interpolsion-get the current point
                Coordinate currentPlace = calculator.CurrentPlace(relative_to, flightPlan, diff);
                flights.Add(new Flight(id, currentPlace.Lng, currentPlace.Lat, flightPlan.Passengers,
                    flightPlan.Company_Name, flightPlan.Initial_Location.Date_Time, false));
            }
            if (flights.Count==0)
            {
                return NotFound();
            }
            else
            {
                return Ok(flights);
            }
            
        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                sql.DeleteRow(id);
                return Ok();
            }
            catch
            {
                return NotFound(id);
            }
        }
    }
}

