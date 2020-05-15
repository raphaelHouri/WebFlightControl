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
        // GET: api/Flights
        [HttpGet]
        public IEnumerable<FlightPlan> GetAllFlights()
        {
            return null;
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

