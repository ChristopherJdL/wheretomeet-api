using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereToMeet.Transporters.Output.GoogleDistanceMatrix
{
    public class DistanceMatrixRowTransporter
    {
        public IEnumerable<DistanceMatrixElementTransporter> Elements { get; set; }
    }
}
