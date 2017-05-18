using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToMeet.Transporters.Input
{
    public class PlacesQueryTransporter
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int Radius { get; set; }
        public IEnumerable<string>  PlacesTypes { get; set; }
    }
}
