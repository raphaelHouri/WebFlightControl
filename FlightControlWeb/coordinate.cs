using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb
{
    public class Coordinate
    {
        private double lat;
        private double lng;
        public Coordinate(double lat, double lng)
        {
            this.lat = lat;
            this.lng = lng;

        }
        public double Lat { get; set; }
        public double Lng { get; set; }



    }
}
