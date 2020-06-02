using FlightControlWeb;
using FlightControlWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTest
{
    class FakeExternalFlight : IExternalFlights
    {
        public List<Flight> changeBoolEX(List<Flight> flights)
        {
            throw new NotImplementedException();
        }

        public Task<FlightPlan> GetExternalFlightById(string id)
        {
            return Task.Run(() =>
            {
                string company = "meshi";
                int passenger = 150;
                string time = "2020-06-01T12:32:00Z";
                DateTime dt = DateTime.Parse(time);
                dt = TimeZoneInfo.ConvertTimeToUtc(dt);
                //create initial location
                InitialLocation initialLocation = new InitialLocation(50, 70, dt);
                List<Segment> segmentsList = new List<Segment>();
                FlightPlan flightPlan = new FlightPlan(passenger, company, initialLocation, segmentsList);

                return flightPlan;
            });
        }

        public Task<List<Flight>> GetExternalFlights(string time)
        {
            throw new NotImplementedException();
        }
    }
}
