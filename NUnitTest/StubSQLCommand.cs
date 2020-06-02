using FlightControlWeb;
using FlightControlWeb.Models;
using System;
using System.Collections.Generic;

namespace NUnitTest
{
    class StubSQLCommand : ISQLCommands

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
            int passengers = 2;
            string company_name = "ISRAIR";
            string time = "2020-06-01T12:32:00Z";
            DateTime dt = DateTime.Parse(time);
            dt = TimeZoneInfo.ConvertTimeToUtc(dt);
            InitialLocation initial_location = new InitialLocation(34.957610, 29.555631, dt);
            List<Segment> segments = segmentList("test123");
            FlightPlan flightPlan = new FlightPlan(passengers, company_name, initial_location, segments);
            return new FlightPlanDB("test123", flightPlan);
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
            List<Segment> segments = new List<Segment>(){
             new Segment(35.211514,31.769399,10000)
            };
            return segments;
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