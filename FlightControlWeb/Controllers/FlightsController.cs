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


        //injection - we should get it in the constructor not new
        // private IProductManager flightManager = new ProductsManger();
        // GET: api/Flights?relative_to=<DATE_TIME>
        [HttpGet]
        public IEnumerable<Flight> GetAllFlights([FromQuery(Name = "relative_to")] string relative_to)
        {
            List<Flight> flights = new List<Flight>();
            FlightCalculator calculator = new FlightCalculator();
            FlightPlan f=null;
            //interpolsion
            //step 1. subset from  current datetime to initial(need to convert the string).
            double diff = calculator.SubTime(f.Initial_Location.DateTime, relative_to);
            //step 2. sum total time the flight take
            double total = calculator.SumTimeSpan(f.Segments);
            //step 3. divide the dattime from the total
            double realtiveTime = diff / total;
            //step 4. check the total distance in the flight
            double totalDis = calculator.TotalDistance(f.Segments);
            //step 5.mult the realtive time to the total distance
            double longitude=1;
            double latitde=1;
            flights.Add(new Flight(f.Id, longitude, latitde, f.Passengers, f.Company_Name, f.Initial_Location.DateTime, false));
            return flights;
        }

        // GET: api/Flights/5
        [HttpGet("{id}", Name = "Get")]
            public string Get(int id)
            {
                return "value";
            }

        // POST: api/Flights
        [HttpPost]
            public FlightPlan Post([FromBody] FlightPlan p)
            {
            // p.Id = 5;
            // return CreatedAtAction(actionName: "GetItem", new { id = p.Id }, p);

            //SQL part
            Database databaseObject = new Database();
            SQLCommands sql = new SQLCommands();
           // sql.addPlan(p, databaseObject);
            return p;
            }
  
        // PUT: api/Flights/5
        [HttpPut("{id}")]
            public void Put(int id, [FromBody] string value)
            {
            }

            // DELETE: api/ApiWithActions/5
            [HttpDelete("{id}")]
            public void Delete(int id)
            {
            }


        }

    }

