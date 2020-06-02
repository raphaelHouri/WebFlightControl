using FlightControlWeb;
using FlightControlWeb.Models;
using System;
using System.Collections.Generic;

namespace NUnitTest
{
    class FakeSQLCommand : ISQLCommands

    {
        public void addListSegmet(FlightPlan flightPlan, string id)
        {
            throw new NotImplementedException();
        }

        public void addPlan(FlightPlan flightPlan)
        {
            throw new NotImplementedException();
        }

        public void addServer(Server server)
        {
            throw new NotImplementedException();
        }

        public string createId()
        {
            throw new NotImplementedException();
        }

        public void deleteRow(string id)
        {
            throw new NotImplementedException();
        }

        public void deleteServer(string id)
        {
            throw new NotImplementedException();
        }

        public List<FlightPlanDB> flightsList(string time)
        {
            throw new NotImplementedException();
        }

        public FlightPlanDB flightsplanById(string id)
        {
            string company = "meshi";
            int passenger = 150;
            //create initial location
            InitialLocation initialLocation = new InitialLocation(50, 70, fromStringToDate("2020 - 06-01T12:32:00Z"));
            //create flightplan
            FlightPlan flightPlan = new FlightPlan(passenger, company, initialLocation, segmentList(id));
            return new FlightPlanDB("1234", flightPlan);
        }

        public DateTime fromStringToDate(string time)
        {
            DateTime dt = DateTime.Parse(time);
            dt = TimeZoneInfo.ConvertTimeToUtc(dt);
            return dt;
        }

        public Coordinate getEndCoors(List<Segment> seg)
        {
            throw new NotImplementedException();
        }

        public DateTime getEndTime(List<Segment> seg, DateTime startTime)
        {
            throw new NotImplementedException();
        }

        public List<Segment> segmentList(string id)
        {
            List<Segment> list = new List<Segment>();
            return list;
        }

        public Server serverById(string id)
        {
            throw new NotImplementedException();
        }

        public List<Server> ServerList()
        {
            throw new NotImplementedException();
        }
    }
}