using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToMeet.Transporters
{
    public class GeoCoordinatesTransporter
    {
        [JsonProperty(PropertyName = "lat")]
        public double X { get; set; }
        [JsonProperty(PropertyName = "lng")]
        public double Y { get; set; }
        public override string ToString()
        {
            string latitudeString = this.Y.ToString(CultureInfo.InvariantCulture);
            string longitudeString = this.X.ToString(CultureInfo.InvariantCulture);
            return $"{latitudeString},{longitudeString}";
        }
    }
}
