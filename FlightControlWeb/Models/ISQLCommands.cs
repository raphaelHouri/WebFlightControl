using FlightControlWeb.Models;
using System;
using System.Collections.Generic;

namespace FlightControlWeb
{
    public interface ISQLCommands
    {
        void addListSegmet(FlightPlan flightPlan, string id);
        void addPlan(FlightPlan flightPlan);
        void addServer(Server server);
        string createId();
        void deleteRow(string id);
        void deleteServer(string id);
        List<FlightPlanDB> flightsList(string time);
        FlightPlanDB flightsplanById(string id);
        DateTime fromStringToDate(string time);
        Coordinate getEndCoors(List<Segment> seg);
        DateTime getEndTime(List<Segment> seg, DateTime startTime);
        List<Segment> segmentList(string id);
        Server serverById(string id);
        List<Server> ServerList();
    }
}