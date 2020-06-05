
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
    public class SQLCommands : ISQLCommands
    {
      private Database databaseObject = new Database();
       
        //add plan to DB from the object we got from json
        public void AddPlan(FlightPlan flightPlan)
        {

            string id = CreateId();
            Coordinate coord = GetEndCoors(flightPlan.Segments);
            //get the end time of the flight
            DateTime endTime = GetEndTime(flightPlan.Segments,
                flightPlan.Initial_Location.Date_Time);
            DateTime statTime = TimeZoneInfo.ConvertTimeToUtc(
                flightPlan.Initial_Location.Date_Time);
            string statTimeString = statTime.ToString("u", DateTimeFormatInfo.InvariantInfo);
            endTime = TimeZoneInfo.ConvertTimeToUtc(endTime);
            string endTimeString = endTime.ToString("u", DateTimeFormatInfo.InvariantInfo);
            ////// INSERT INTO DATABASE
            using var myCommand = new SQLiteCommand(databaseObject.myConnection);
            string qurey = " VALUES(" + "\'" + id + "\',\'" + flightPlan.Company_Name
                + "\'," + flightPlan.Passengers + ",\'" + statTimeString + "\'," +
                flightPlan.Initial_Location.Longitude + "," 
                + flightPlan.Initial_Location.Latitude  + ","
                + coord.Lat + "," + coord.Lng + ",\'" + endTimeString + "\'" + ")";
            myCommand.CommandText = "INSERT INTO Flight(id, company, passengers," +
                "start_time,start_longitude, start_latitude,end_latitude, " +
                "end_longitude, end_time)" + qurey;
            int result = myCommand.ExecuteNonQuery();
            if (result > 0)
            {
                AddListSegmet(flightPlan, id);
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine(result);
            }

        }

        //add list of segment by id to the DB
        public void AddListSegmet(FlightPlan flightPlan, string id)
        {

            int length = flightPlan.Segments.Count;
            for (int i = 0; i < length; i++)
            {
                using var myCommand = new SQLiteCommand(databaseObject.myConnection);
                string qurey = " VALUES(" + "\'" + id + "\'," + i + "," +
                    flightPlan.Segments[i].Longitude + "," + flightPlan.Segments[i].Latitude
                    + "," + flightPlan.Segments[i].Timespan_Seconds + ")";
                myCommand.CommandText = "INSERT INTO Segments(id, serial, longitude, latitude," +
                    " timespan)" + qurey;
                int result = myCommand.ExecuteNonQuery();
                if (result > 0)
                {
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine(result);
                }
                //databaseObject.CloseConnection();

            }

        }
        //delete row details from the two table
        public void DeleteRow(string id)
        {

            // delete flight from flight table bty id
            string stm = $"DELETE FROM Flight WHERE id= '{id}'";

            using var myCommand1 = new SQLiteCommand(stm, databaseObject.myConnection);
            int result = myCommand1.ExecuteNonQuery();
            if (result > 0)
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine(result);
            }
            //databaseObject.CloseConnection();
            // delete segments from segments table by id

             stm = $"DELETE FROM Segments WHERE id='{id}'";

            using var myCommand2 = new SQLiteCommand(stm, databaseObject.myConnection);
            int s = myCommand2.ExecuteNonQuery();
            if (result > 0)
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine(result);
            }
            //databaseObject.CloseConnection();


        }
        //return list of segments
        public List<Segment> SegmentList(string id)
        {
            Segment segment;
            // SELECT FROM DATABASE
            string query = $"SELECT * FROM Segments WHERE id = '{id}'  ORDER BY serial ASC;";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            ////databaseObject.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            // Creating a List of coordinate
            List<Segment> segmentList = new List<Segment>();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    //get row after row from the DB
                    segment = new Segment(Convert.ToDouble($"{result["longitude"]}"),
                        Convert.ToDouble($"{result["latitude"]}"),
                        Convert.ToDouble($"{result["timespan"]}"));
                    segmentList.Add(segment);
                }
            }
            //databaseObject.CloseConnection();
            return segmentList;

        }
        // return all flight plans in the current time
        public List<FlightPlanDB> FlightsList(string time)
        {   //SQL part
            List<FlightPlanDB> flightsList = new List<FlightPlanDB>();
            //InitialLocation initialLocation;
            FlightPlanDB flightPlanDB;
            //FlightPlan flightPlan;
            //string query = $"SELECT * FROM Flight WHERE '{time}'> start_time ";
            string query =
                $"SELECT * FROM Flight WHERE ('{time}'>= start_time) AND ('{time}' <= end_time)";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            ////databaseObject.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            // Creating a List of integers

            if (result.HasRows)
            {

                while (result.Read())
                {
                    //get id
                    string id = $"{result["id"]}";
                    flightPlanDB = FlightsPlanById(id);
                    flightsList.Add(flightPlanDB);
                }
            }
            //databaseObject.CloseConnection();


            return flightsList;

        }
        public FlightPlanDB FlightsPlanById(string id)
        {
            InitialLocation initialLocation;
            FlightPlanDB flightPlanDB = null;
            FlightPlan flightPlan;
            string query = $"SELECT * FROM Flight WHERE id = '{id}'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            ////databaseObject.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            // Creating a List of integers

            if (result.HasRows)
            {

                while (result.Read())
                {
                    string company = $"{result["company"]}";
                    int passenger = Convert.ToInt32($"{result["passengers"]}");
                    //create initial location
                    initialLocation = new InitialLocation(Convert.ToDouble(
                        $"{result["start_longitude"]}"),
                        Convert.ToDouble($"{result["start_latitude"]}"),
                        FromStringToDate($"{result["start_time"]}"));
                    //create flightplan
                    flightPlan = new FlightPlan(passenger, company,
                        initialLocation, SegmentList(id));
                    flightPlanDB = new FlightPlanDB(id, flightPlan);

                }
            }
            //databaseObject.CloseConnection();


            return flightPlanDB;

        }


        
        public string CreateId()
        {
            RandomGenerator generator = new RandomGenerator();
            return generator.RandomPassword();
        }
        //get the end coord
        public Coordinate GetEndCoors(List<Segment> seg)
        {
            int lastSegIndex = seg.Count - 1;
            Coordinate coord = new Coordinate(seg[lastSegIndex].Latitude,
                seg[lastSegIndex].Latitude);
            return coord;
        }
        //get end time of the flight
        public DateTime GetEndTime(List<Segment> seg, DateTime startTime)
        {
            double sumSeconds = 0;
            foreach (Segment item in seg)
            {
                sumSeconds += item.Timespan_Seconds;
            }
            return startTime.AddSeconds(sumSeconds);

        }
        //convert string to date object
        public DateTime FromStringToDate(string time)
        {

            DateTime dt = DateTime.Parse(time);
            dt = TimeZoneInfo.ConvertTimeToUtc(dt);

            return dt;
        }



        //server table
        public void AddServer(Server server)
        {

            ////// INSERT INTO DATABASE

            using var cmd = new SQLiteCommand(databaseObject.myConnection);
            string a = " VALUES(" + "\'" + server.ServerId + "\',\'" + server.ServerUrl + "\')";
            cmd.CommandText = "INSERT INTO Servers(id,url)" + a;

            int result = cmd.ExecuteNonQuery();

            if (result > 0)
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine(result);
            }
            //databaseObject.CloseConnection();
        }


        public void DeleteServer(string id)
        {

            string stm = $"DELETE FROM Servers WHERE id = '{id}'";

            using var cmd1 = new SQLiteCommand(stm, databaseObject.myConnection);
            int result = cmd1.ExecuteNonQuery();
            if (result > 0)
            {
                Console.WriteLine(result);
            }
            else
            {
                //databaseObject.CloseConnection();
                throw new Exception();
            }
            //databaseObject.CloseConnection();
        }
        public List<Server> ServerList()
        {   //SQL part
            List<Server> ServerList = new List<Server>();
            Server server = null;
            Database databaseObject = new Database();
            string query = $"SELECT * FROM Servers";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            ////databaseObject.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            // Creating a List of integers

            if (result.HasRows)
            {

                while (result.Read())
                {
                    //get id
                    string id = $"{result["id"]}";
                    string url = $"{result["url"]}";

                    server = ServerById($"{result["id"]}");
                    ServerList.Add(server);
                }
            }
            //databaseObject.CloseConnection();


            return ServerList;

        }
        //return server url by id
        public Server ServerById(string id)
        {
            Server server = null;
            
            string query = $"SELECT * FROM Servers WHERE id = '{id}';";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            ////databaseObject.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    string url = $"{result["url"]}";

                    server = new Server(id, url);

                }
            }
            //databaseObject.CloseConnection();
            return server;

        }
    }
}


