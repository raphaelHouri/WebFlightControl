using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlanDB
    {
        private FlightPlan flightPlan;
        private string id;

        public FlightPlanDB(string id, FlightPlan flightPlan)
        {
            this.id = id;
            this.flightPlan = flightPlan;

        }

        public string GetId()
        {
            return this.id;
        }
        public FlightPlan GetFlightPlan()
        {
            return this.flightPlan;
        }
    }
}
