using FlightControlWeb.Models;
using System;
using System.Collections.Generic;

namespace FlightControlWeb
{
    public interface ISQLCommands
    {
        void AddListSegmet(FlightPlan flightPlan, string id);
        void AddPlan(FlightPlan flightPlan);
        void AddServer(Server server);
        string CreateId();
        void DeleteRow(string id);
        void DeleteServer(string id);
        List<FlightPlanDB> FlightsList(string time);
        FlightPlanDB FlightsPlanById(string id);
        DateTime FromStringToDate(string time);
        Coordinate GetEndCoors(List<Segment> seg);
        DateTime GetEndTime(List<Segment> seg, DateTime startTime);
        List<Segment> SegmentList(string id);
        Server ServerById(string id);
        List<Server> ServerList();
    }
}