﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{

    //interpolsion
    //
    //step 2. sum total time the flight take
    //step 3. divide the dattime from the total
    //step 4. check the total distance in the flight
    //step 5.mult the realtive time to the total distance
    public class FlightCalculator
    {

        // subset from  current datetime to initial(need to convert the string).
        public double SubTime(DateTime initial, string current) {
            DateTime currentD = this.ConvertStringToDateTime(current);
            return 1;
            }
        public DateTime ConvertStringToDateTime(string time){
            return new DateTime();
            }
         // sum total time the flight take
        public double SumTimeSpan(Segment[] segments,int index)
        {
            double sum = 0;
            for (int i = 0; i < index; i++)
            {
                sum = sum + segments[i].TimespanSeconds;
            }
            return sum;
        }

        //find the spesific segment
        public int findIndexSegment(Segment[] segments, double diff)
        {
            double time = 0;
            int index = 0;
            for(int i = 0; i < segments.Length; i++)
            {
                if (time > diff)
                {
                    index = i;
                    break;
                }
                else if (time == diff)
                {
                    i = i + 1;
                    break;
                }
                else
                {
                    time = time + segments[i].TimespanSeconds;
                }
            }
            return index;
           
        }
        public Coordinate CurrentPlace(string current,Segment[] s,double diff)
        {
            int findIndex = this.findIndexSegment(s, diff);
            //add condition if ifind=0;
            double latStart = s[findIndex - 1].Latitde;
            double longStart = s[findIndex - 1].Longitude;
            double latEnd = s[findIndex].Latitde;
            double longEnd = s[findIndex].Longitude;
            //use divide line to parts equation 
            //find the time from the begin to the current in the spesfic segment
            double left = diff - SumTimeSpan(s, findIndex);
            //find the left time 
            double right = s[findIndex].TimespanSeconds - left;
            double currentlat = (latStart * right + latEnd * left) / (left+right);
            double currentlong = (longStart * right + longEnd * left) / (left+right);
            Coordinate currentP = new Coordinate(currentlat,currentlong);
            return currentP;
        }
      

    }
}