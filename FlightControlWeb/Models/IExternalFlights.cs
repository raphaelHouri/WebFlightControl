using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IExternalFlights
    {
        List<Flight> changeBoolEX(List<Flight> flights);
        Task<FlightPlan> GetExternalFlightById(string id);
        Task<List<Flight>> GetExternalFlights(string time);
    //    Task<FlightPlan> GetFlightPlanIdNoError(string id);
    }
}