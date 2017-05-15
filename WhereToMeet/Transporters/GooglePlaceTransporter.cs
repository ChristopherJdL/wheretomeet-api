using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhereToMeet.Transporters.Output.GooglePlaces;

namespace WhereToMeet.Transporters.Output.GooglePlaces
{
    public class GooglePlaceTransporter
    {
        public string Name { get; set; }
        public GeometryTransporter Geometry { get; set; }
        public IEnumerable<string> Types { get; set; }

        public static explicit operator PlaceTransporter(GooglePlaceTransporter gpt)
        {
            return new PlaceTransporter()
            {
                Name = gpt.Name,
                Tags = gpt.Types,
                Latitude = gpt.Geometry.Location.Y,
                Longitude = gpt.Geometry.Location.X,
            };
        }
    }
}
