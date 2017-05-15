using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToMeet.Transporters.Output.GooglePlaces
{
    public class GooglePlacesResponse
    {
        public string Status { get; set; }
        public IEnumerable<GooglePlaceTransporter> Results { get; set; }

    }
}
