using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToMeet.Transporters.Output.GoogleDistanceMatrix
{
    public class DistanceTransporter
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
    public class DurationTransporter
    {
        /// <summary>
        /// The duration, in seconds.
        /// </summary>
        public int Value { get; set; }
        public string Text { get; set; }
    }
    public class DistanceMatrixElementTransporter
    {
        public DistanceTransporter Distance { get; set; }

        public DurationTransporter Duration { get; set; }
        public string Status { get; set; }
    }

}
