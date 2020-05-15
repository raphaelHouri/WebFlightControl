using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb
{
    public class SQLCommands
    {
        public void addPlan(FlightPlan flightPlan)
        {
            
            Database databaseObject = new Database();
            string id = createId();
            Coordinate coord = getEndCoors(flightPlan.Segments);
            DateTime endTime = getEndTime(flightPlan.Segments, flightPlan.Initial_Location.DateTime);

            ////// INSERT INTO DATABASE
            string query = "INSERT INTO Flight ('id', 'start_latitude','start_longitude','end_latitude','end_longitude'," +
                " 'start_time', 'end_time', 'company_name', 'passengers')" +
                " VALUES (@id, @flightPlan., @flightPlan.company_name, @flightPlan.longitude, @flightPlan.latitude, @flightPlan.date_time);";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            myCommand.Parameters.AddWithValue("@id", id);
            myCommand.Parameters.AddWithValue("@start_latitude", flightPlan.Initial_Location.Latitude);
            myCommand.Parameters.AddWithValue("@start_longitude", flightPlan.Initial_Location.Longitude);
            myCommand.Parameters.AddWithValue("@end_latitude", coord.Lat);
            myCommand.Parameters.AddWithValue("@end_longitude", coord.Lat);
            myCommand.Parameters.AddWithValue("@start_time", flightPlan.Initial_Location.DateTime);
            myCommand.Parameters.AddWithValue("@end_time", endTime);
            myCommand.Parameters.AddWithValue("@company_name", flightPlan.Company_Name);
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
