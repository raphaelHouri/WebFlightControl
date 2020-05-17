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
        public void addPlan(FlightPlan flightPlan, Database databaseObject)
        {
            
            string id = createId();

            Coordinate coord = getEndCoors(flightPlan.Segments);
            DateTime endTime = getEndTime(flightPlan.Segments, flightPlan.Initial_Location.DateTime);

            //fix time to utc and creat string
            DateTime statTime = TimeZone.CurrentTimeZone.ToUniversalTime(flightPlan.Initial_Location.DateTime);
            string statTimeString = statTime.ToString("f", DateTimeFormatInfo.InvariantInfo);
            endTime = TimeZone.CurrentTimeZone.ToUniversalTime(endTime);
            string endTimeString = endTime.ToString("f", DateTimeFormatInfo.InvariantInfo);
            addListSegmet(flightPlan, databaseObject, id);
            ////// INSERT INTO DATABASE
            string query = "INSERT INTO Flight ('id', 'start_latitude','start_longitude','end_latitude','end_longitude','start_time','end_time', 'company', 'passengers') VALUES (@id, @start_latitude, @start_longitude, @end_latitude, @end_longitude ,@start_time, @end_time, @company, @passengers );";
                                                                                                   
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            myCommand.Parameters.AddWithValue("@id", id);
            myCommand.Parameters.AddWithValue("@start_latitude", flightPlan.Initial_Location.Latitude);
            myCommand.Parameters.AddWithValue("@start_longitude", flightPlan.Initial_Location.Longitude);
            myCommand.Parameters.AddWithValue("@end_latitude", coord.Lat);
            myCommand.Parameters.AddWithValue("@end_longitude", coord.Lat);
            myCommand.Parameters.AddWithValue("@start_time", statTimeString);
            myCommand.Parameters.AddWithValue("@end_time", endTimeString);
            myCommand.Parameters.AddWithValue("@company", flightPlan.Company_Name);
            myCommand.Parameters.AddWithValue("@passengers", flightPlan.Passenger);
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

            Console.WriteLine("Rows Added : {0}", result);
            string userName = Console.ReadLine();
        }
        public void addListSegmet(FlightPlan flightPlan, Database databaseObject, string id)
        {
            int length = flightPlan.Segments.Length;
            for (int i=0;i<length;i++ )
            {
                string query = "INSERT INTO Segments ('id', 'serial','longitude', 'latitude','timespan') VALUES (@id, @serial, @longitude, @latitude, @timespan );";
                SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);


                databaseObject.OpenConnection();
                myCommand.Parameters.AddWithValue("@id", id);
                myCommand.Parameters.AddWithValue("@serial", i);
                myCommand.Parameters.AddWithValue("@longitude", flightPlan.Segments[i].Longitude);
                myCommand.Parameters.AddWithValue("@latitude", flightPlan.Segments[i].Latitde);
                myCommand.Parameters.AddWithValue("@timespan", flightPlan.Segments[i].TimespanSeconds);
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

            public string createId()
        {
            RandomGenerator generator = new RandomGenerator();
            return generator.RandomPassword();
        }
        public Coordinate getEndCoors(Segment[] seg)
        {
            int lastSegIndex = seg.Length - 1;
            Coordinate coord = new Coordinate(seg[lastSegIndex].Latitde, seg[lastSegIndex].Latitde);
            return coord;
        }
        public DateTime getEndTime(Segment[] seg, DateTime startTime)
        {
            int sumSeconds = 0;
            for(int i=0 ; i < seg.Length; i++)
            {
                sumSeconds += seg[i].TimespanSeconds;
            }
            return startTime.AddSeconds(sumSeconds);

        }


    }
}
