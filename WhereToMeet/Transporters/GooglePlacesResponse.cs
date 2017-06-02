using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToMeet.Transporters.Output.GooglePlaces
{
    public class GooglePlacesResponse
    {
        [JsonProperty(PropertyName = "next_page_token")]
        public string NextPageToken { get; set; }
        public string Status { get; set; }
        public IEnumerable<GooglePlaceTransporter> Results { get; set; }

    }
}
