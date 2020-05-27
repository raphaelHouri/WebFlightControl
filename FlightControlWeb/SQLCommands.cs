using FlightControlWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb
{
    public class SQLCommands
    {
        private Database databaseObject = new Database();
        //add plan to DB from the object we got from json
        public void addPlan(FlightPlan flightPlan)
        {
            string id = createId();
            Coordinate coord = getEndCoors(flightPlan.Segments);
            DateTime endTime = getEndTime(flightPlan.Segments, flightPlan.Initial_Location.Date_Time);

            DateTime statTime = TimeZoneInfo.ConvertTimeToUtc(flightPlan.Initial_Location.Date_Time);
            string statTimeString = statTime.ToString("u", DateTimeFormatInfo.InvariantInfo);
            endTime = TimeZoneInfo.ConvertTimeToUtc(endTime);
            string endTimeString = endTime.ToString("u", DateTimeFormatInfo.InvariantInfo);

            ////// INSERT INTO DATABASE
            string query = "INSERT INTO Flight ('id', 'start_latitude','start_longitude','end_latitude','end_longitude','start_time','end_time', 'company', 'passengers') VALUES (@id, @start_latitude, @start_longitude, @end_latitude, @end_longitude ,@start_time, @end_time, @company, @passengers );";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            myCommand.Parameters.AddWithValue("@id", id);
            myCommand.Parameters.AddWithValue("@start_latitude", flightPlan.Initial_Location.Latitude);
            myCommand.Parameters.AddWithValue("@start_longitude", flightPlan.Initial_Location.Longitude);
            myCommand.Parameters.AddWithValue("@end_latitude", coord.Lat);
            myCommand.Parameters.AddWithValue("@end_longitude", coord.Lng);
            myCommand.Parameters.AddWithValue("@start_time", statTimeString);
            myCommand.Parameters.AddWithValue("@end_time", endTimeString);
            myCommand.Parameters.AddWithValue("@company", flightPlan.Company_Name);
            myCommand.Parameters.AddWithValue("@passengers", flightPlan.Passengers);
            int result = myCommand.ExecuteNonQuery();
            if (result > 0)
            {
                addListSegmet(flightPlan, id);
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine(result);
            }
            databaseObject.CloseConnection();
        }

        //add list of segment by id to the DB
        public void addListSegmet(FlightPlan flightPlan, string id)
        {
            int length = flightPlan.Segments.Count;
            for (int i = 0; i < length; i++)
            {
                string query = "INSERT INTO Segments ('id', 'serial','longitude', 'latitude','timespan') VALUES (@id, @serial, @longitude, @latitude, @timespan );";
                SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);


                databaseObject.OpenConnection();
                myCommand.Parameters.AddWithValue("@id", id);
                myCommand.Parameters.AddWithValue("@serial", i);
                myCommand.Parameters.AddWithValue("@longitude", flightPlan.Segments[i].Longitude);
                myCommand.Parameters.AddWithValue("@latitude", flightPlan.Segments[i].Latitude);
                myCommand.Parameters.AddWithValue("@timespan", flightPlan.Segments[i].Timespan_Seconds);
                int result = myCommand.ExecuteNonQuery();
                if (result > 0)
                {
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine(result);
                }
                databaseObject.CloseConnection();

            }

        }
        //delete row details from the two table
        public void deleteRow(string id)
        {

            string query = $"DELETE FROM Flight WHERE id = '{id}';";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            int result = myCommand.ExecuteNonQuery();
            if (result > 0)
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine(result);
            }
            databaseObject.CloseConnection();
            query = $"DELETE FROM Segments WHERE id = '{id}';";
            myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            result = myCommand.ExecuteNonQuery();
            if (result > 0)
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine(result);
            }
            databaseObject.CloseConnection();


        }
        //return list of segments
        public List<Segment> segmentList(string id)
        {
            Segment segment;
            // SELECT FROM DATABASE

            string query = $"SELECT * FROM Segments WHERE id = '{id}'  ORDER BY serial ASC;";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            // Creating a List of coordinate
            List<Segment> segmentList = new List<Segment>();
            if (result.HasRows)
            {
                while (result.Read())
                {

                    segment = new Segment(Convert.ToDouble($"{result["longitude"]}"), Convert.ToDouble($"{result["latitude"]}"), Convert.ToDouble($"{result["timespan"]}"));
                    segmentList.Add(segment);
                }
            }
            databaseObject.CloseConnection();
            return segmentList;

        }
        // return all flight plans in the current time
        public List<FlightPlanDB> flightsList(string time)
        {   //SQL part
            List<FlightPlanDB> flightsList = new List<FlightPlanDB>();
            Database databaseObject = new Database();
            //InitialLocation initialLocation;
            FlightPlanDB flightPlanDB;
            //FlightPlan flightPlan;
            //string query = $"SELECT * FROM Flight WHERE '{time}'> start_time ";
            string query = $"SELECT * FROM Flight WHERE ('{time}'>= start_time) AND ('{time}' <= end_time)";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            // Creating a List of integers

            if (result.HasRows)
            {

                while (result.Read())
                {
                    //get id
                    string id = $"{result["id"]}";

                    flightPlanDB = flightsplanById(id);
                    flightsList.Add(flightPlanDB);
                }
            }
            databaseObject.CloseConnection();


            return flightsList;

        }
        public FlightPlanDB flightsplanById(string id)
        {
            Database databaseObject = new Database();
            InitialLocation initialLocation;
            FlightPlanDB flightPlanDB = null;
            FlightPlan flightPlan;
            string query = $"SELECT * FROM Flight WHERE id = '{id}'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            // Creating a List of integers

            if (result.HasRows)
            {

                while (result.Read())
                {
                    string company = $"{result["company"]}";
                    int passenger = Convert.ToInt32($"{result["passengers"]}");
                    //create initial location
                    initialLocation = new InitialLocation(Convert.ToDouble($"{result["start_latitude"]}"), Convert.ToDouble($"{result["start_longitude"]}"), fromStringToDate($"{result["start_time"]}"));
                    //create flightplan
                    flightPlan = new FlightPlan(passenger, company, initialLocation, segmentList(id));

                    flightPlanDB = new FlightPlanDB(id, flightPlan);

                }
            }
            databaseObject.CloseConnection();


            return flightPlanDB;

        }



        public string createId()
        {
            RandomGenerator generator = new RandomGenerator();
            return generator.RandomPassword();
        }
        public Coordinate getEndCoors(List<Segment> seg)
        {
            int lastSegIndex = seg.Count - 1;
            Coordinate coord = new Coordinate(seg[lastSegIndex].Latitude, seg[lastSegIndex].Latitude);
            return coord;
        }
        public DateTime getEndTime(List<Segment> seg, DateTime startTime)
        {
            double sumSeconds = 0;
            foreach (Segment item in seg)
            {
                sumSeconds += item.Timespan_Seconds;
            }
            return startTime.AddSeconds(sumSeconds);

        }
        public DateTime fromStringToDate(string time)
        {

            DateTime dt = DateTime.Parse(time);
            dt = TimeZoneInfo.ConvertTimeToUtc(dt);

            return dt;
        }



        //server table
        public void addServer(Server server)
        {

        //    Database databaseObject = new Database();
            ////// INSERT INTO DATABASE
            string query = "INSERT INTO Servers ('id', 'url') VALUES (@id, @url);";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            myCommand.Parameters.AddWithValue("@id", server.ServerId);
            myCommand.Parameters.AddWithValue("@url", server.ServerUrl);

            int result = myCommand.ExecuteNonQuery();
            if (result > 0)
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine(result);
            }
            databaseObject.CloseConnection();
        }


        public void deleteServer(string id)
        {

            string query = $"DELETE FROM Servers WHERE id = '{id}';";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            int result = myCommand.ExecuteNonQuery();
            if (result > 0)
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine(result);
            }
            databaseObject.CloseConnection();
        }
        public List<Server> ServerList()
        {   //SQL part
            List<Server> ServerList = new List<Server>();
            Server server = null;
            Database databaseObject = new Database();
            string query = $"SELECT * FROM Servers";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            // Creating a List of integers

            if (result.HasRows)
            {

                while (result.Read())
                {
                    //get id
                    string id = $"{result["id"]}";
                    string url = $"{result["url"]}";

                    server = new Server(id, url);
                    ServerList.Add(server);
                }
            }
            databaseObject.CloseConnection();


            return ServerList;

        }
        public Server serverById(string id)
        {
            Server server = null;
            string query = $"SELECT * FROM Servers WHERE id = '{id}';";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            
            if (result.HasRows)
            {
                while (result.Read())
                {
                    string url = $"{result["url"]}";

                    server = new Server(id, url);

                }
            }
            databaseObject.CloseConnection();
            return server;

        }




    }



}