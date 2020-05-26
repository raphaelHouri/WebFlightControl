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
    /*
        let welcome = Welcome.FromJson(jsonString);*/

    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private FlightCalculator calculator = new FlightCalculator();
        //SQL part
        private SQLCommands sql = new SQLCommands();

        //injection - we should get it in the constructor not new
        // private IProductManager flightManager = new ProductsManger();
        // GET: api/Flights?relative_to=<DATE_TIME>
        [HttpGet]
        public async Task<IEnumerable<Flight>> GetAllFlights([FromQuery(Name = "relative_to")] string relative_to)
        {
            bool checkSyncAll = Request.Query.ContainsKey("sync_all");
            if (checkSyncAll)
            {
                //need to get flights from out servers
            }
            //the list of flighs we will send to the cliet to update the markers
            List<Flight> flights = new List<Flight>();
            //we need to get from the db all the flight plan that are relvante
          
            List<FlightPlanDB> flightList = sql.flightsList(relative_to);

            for (int i = 0; i < flightList.Count; i++)
            {
                string id = flightList[i].GetId();
                FlightPlan flightPlan = flightList[i].GetFlightPlan();

                //step 1. subset from  current datetime to initial(need to convert the string).
                double diff = calculator.SubTime(flightPlan.Initial_Location.Date_Time, relative_to);
                //interpolsion-get the current point
                Coordinate currentPlace = calculator.CurrentPlace(relative_to, flightPlan, diff);
                flights.Add(new Flight(id, currentPlace.Lng, currentPlace.Lat, flightPlan.Passengers, flightPlan.Company_Name, flightPlan.Initial_Location.Date_Time, false));
            }
            return flights;
        }

        // GET: api/Flights/5
        //[HttpGet("{id}", Name = "Get")]
        //    public string Get(string id)
        //    {
        //        return "value";
        //    }



        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
            public void Delete(string id)
            {
                
                sql.deleteRow(id);

            }


        }

    }

