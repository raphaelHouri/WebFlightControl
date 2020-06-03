using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IExternalFlights
    {
        List<Flight> changeBoolEX(List<Flight> flights);
        void DeleteDic(string id);
        Task<FlightPlan> GetExternalFlightById(string id);
        Task<List<Flight>> GetExternalFlights(string time);
    }
}