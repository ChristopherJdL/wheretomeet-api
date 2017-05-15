using System.Collections.Generic;

namespace WhereToMeet.Transporters.Output
{
    public class PlaceTransporter
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}