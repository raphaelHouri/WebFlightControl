using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class FlightPlanDb
    {
        private FlightPlan flightPlan;
        private string id;

        public FlightPlanDb(FlightPlan flightPlan,string id)
        {
            this.flightPlan = flightPlan;
            this.id = id;
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
