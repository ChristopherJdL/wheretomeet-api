using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToMeet.Transporters.Output.GoogleDistanceMatrix
{
    public class DistanceMatrixResponse
    {
        [JsonProperty(PropertyName = "destination_addresses")]
        public string[] DestinationAddresses { get; set; }

        [JsonProperty(PropertyName = "origin_addresses")]
        public string[] OriginAddresses { get; set; }

        public IEnumerable<DistanceMatrixRowTransporter> Rows { get; set; }
        public string Status { get; set; }
    }
}
