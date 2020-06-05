using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace FlightControlWeb
{
    public class Database
    {

        public SQLiteConnection myConnection;
        public Database()
        {
            string fileName = "DataSource=database.db;";
            if (!File.Exists("./database.db"))
            {
                //open connection
                myConnection = new SQLiteConnection(fileName);
                myConnection.Open();
                try
                {
                    //build the tables
                    BuildFlight();
                    BuildSegments();
                    BuildServers();
                }
                catch
                {
                    Console.WriteLine("build DB feiled");
                }
            }
            else
            {
                myConnection = new SQLiteConnection(fileName);
                myConnection.Open();
            }
        }

        private void BuildFlight()
        {
            using var myCommand = new SQLiteCommand(myConnection);
            //create table
            myCommand.CommandText = "DROP TABLE IF EXISTS Flight";
            int result = myCommand.ExecuteNonQuery();
            //create the query for building the row
            myCommand.CommandText = @"CREATE TABLE Flight(id TEXT PRIMARY KEY,
                    company TEXT, passengers INT, start_time TEXT, end_time TEXT, 
                    start_longitude DOUBLE,start_latitude DOUBLE,
                    end_latitude DOUBLE, end_longitude DOUBLE)";

            result = myCommand.ExecuteNonQuery();
        }
        private void BuildSegments()
        {
            using var myCommand = new SQLiteCommand(myConnection);
            //create table
            myCommand.CommandText = "DROP TABLE IF EXISTS Segments";
            int result = myCommand.ExecuteNonQuery();
            //create the query for building the row
            myCommand.CommandText = @"CREATE TABLE Segments(id TEXT, serial INT,
                    longitude DOUBLE, latitude DOUBLE, timespan INT)";
            result = myCommand.ExecuteNonQuery();
        }


        private void BuildServers()
        {
            using var myCommand = new SQLiteCommand(myConnection);
            //create table
            myCommand.CommandText = "DROP TABLE IF EXISTS Servers";
            int result = myCommand.ExecuteNonQuery();
            //create the query for building the row

            myCommand.CommandText = @"CREATE TABLE Servers(
                id TEXT NOT NULL, url TEXT NOT NULL,
                PRIMARY KEY(id, url)
                );";
            result = myCommand.ExecuteNonQuery();
        }


        //public void OpenConnection()
        //{
        //    if (myConnection.State != System.Data.ConnectionState.Open)
        //    {
        //        myConnection.Open();
        //    }
        //}

        //public void CloseConnection()
        //{
        //    if (myConnection.State != System.Data.ConnectionState.Closed)
        //    {
        //        myConnection.Close();
        //    }
        //}
    }
}
