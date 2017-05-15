using WhereToMeet.Transporters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheretoMeet.Services
{
    public interface IDistanceResolver
    {
        Task<int> ResolveDistance(GeoCoordinatesTransporter origin, GeoCoordinatesTransporter destination);
        Task<int> ResolveDuration(GeoCoordinatesTransporter origin, GeoCoordinatesTransporter destination);
    }
}
