namespace FlightControlWeb.Models
{
    public interface IFlightPlanDB
    {
        FlightPlan GetFlightPlan();
        string GetId();
    }
}