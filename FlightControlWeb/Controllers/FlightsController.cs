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

        //injection - we should get it in the constructor not new
        // private IProductManager flightManager = new ProductsManger();
        // GET: api/Flights?relative_to=<DATE_TIME>
        [HttpGet]
        public IEnumerable<Flight> GetAllFlights([FromQuery(Name = "relative_to")] string relative_to)
        {
            bool checkSyncAll = Request.Query.ContainsKey("sync_all");
            if (checkSyncAll)
            {
                //need to get flights from out servers
            }
            //the list of flighs we will send to the cliet to update the markers
            List<Flight> flights = new List<Flight>();
            //we need to get from the db all the flight plan that are relvante
            List<FlightPlan> f = null;

            for (int i = 0; i < f.Count; i++)
            {
                //step 1. subset from  current datetime to initial(need to convert the string).
                double diff = calculator.SubTime(f[i].Initial_Location.Date_Time, relative_to);
                //interpolsion-get the current point
                Coordinate currentPlace = calculator.CurrentPlace(relative_to, f[i].Segments, diff);
                flights.Add(new Flight(f[i].Id, currentPlace.Lng, currentPlace.Lat, f[i].Passengers, f[i].Company_Name, f[i].Initial_Location.Date_Time, false));
            }
            return flights;
        }

        // GET: api/Flights/5
        [HttpGet("{id}", Name = "Get")]
            public string Get(int id)
            {
                return "value";
            }
    


            // DELETE: api/ApiWithActions/5
            [HttpDelete("{id}")]
            public void Delete(int id)
            {
            }


        }

    }

