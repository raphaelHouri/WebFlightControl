using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb
{
    public class Coordinate
    {
        private double lng;
        private double lat;

        public Coordinate(double lat, double lng)
        {
            this.lat = lat;
            this.lng = lng;

        }

        public double Lat
        {
            set
            {
                this.lat = value;
            }
            get
            {
                return lat;
            }
        }
        public double Lng
        {
            set
            {
                this.lng = value;
            }
            get
            {
                return lng;
            }
        }
    }
}
