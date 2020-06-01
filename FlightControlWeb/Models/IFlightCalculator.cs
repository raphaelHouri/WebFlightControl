using System;
using System.Collections.Generic;

namespace FlightControlWeb.Models
{
    public interface IFlightCalculator
    {
        DateTime ConvertStringToDateTime(string time);
        Coordinate CurrentPlace(string current, FlightPlan flightPlan, double diff);
        int findIndexSegment(List<Segment> segments, double diff);
        double SubTime(DateTime startTime, string current);
        double SumTimeSpan(List<Segment> segments, int index);
    }
}