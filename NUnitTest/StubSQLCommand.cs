using FlightControlWeb;
using FlightControlWeb.Models;
using System;
using System.Collections.Generic;

namespace NUnitTest
{
    class StubSQLCommand : ISQLCommands

    {
        public void AddListSegmet(FlightPlan flightPlan, string id)
        {
            throw new NotImplementedException();
        }

        public void AddPlan(FlightPlan flightPlan)
        {
            throw new NotImplementedException();
        }

        public void AddServer(Server server)
        {
            throw new NotImplementedException();
        }

        public string CreateId()
        {
            throw new NotImplementedException();
        }

        public void DeleteRow(string id)
        {
            throw new NotImplementedException();
        }

        public void DeleteServer(string id)
        {
            throw new NotImplementedException();
        }

        public List<FlightPlanDB> FlightsList(string time)
        {
            throw new NotImplementedException();
        }

        public FlightPlanDB FlightsPlanById(string id)
        {
            if (id == "test1")
            {
                int passengers = 2;
                string company_name = "ISRAIR";
                string time = "2020-06-01T12:32:00Z";
                DateTime dt = DateTime.Parse(time);
                dt = TimeZoneInfo.ConvertTimeToUtc(dt);
                InitialLocation initial_location = new InitialLocation(34.957610, 29.555631, dt);
                List<Segment> segments = SegmentList("test1");
                FlightPlan flightPlan = new FlightPlan(passengers, company_name,
                    initial_location, segments);
                return new FlightPlanDB("test1", flightPlan);
            }
            //test 2
            else
            {
                return null;
            }
        }

        public DateTime FromStringToDate(string time)
        {
            DateTime dt = DateTime.Parse(time);
            dt = TimeZoneInfo.ConvertTimeToUtc(dt);
            return dt;
        }

        public Coordinate GetEndCoors(List<Segment> seg)
        {
            throw new NotImplementedException();
        }

        public DateTime GetEndTime(List<Segment> seg, DateTime startTime)
        {
            throw new NotImplementedException();
        }

        public List<Segment> SegmentList(string id)
        {
            List<Segment> segments = new List<Segment>(){
             new Segment(35.211514,31.769399,10000)
            };
            return segments;
        }

        public Server ServerById(string id)
        {
            throw new NotImplementedException();
        }

        public List<Server> ServerList()
        {
            throw new NotImplementedException();
        }
    }
}