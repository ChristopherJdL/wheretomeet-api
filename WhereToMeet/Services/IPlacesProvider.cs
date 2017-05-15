using WhereToMeet.Transporters.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhereToMeet.Transporters.Output;

namespace WheretoMeet.Services.PlacesProviders
{
    public interface IPlacesProvider
    {
        Task<IEnumerable<PlaceTransporter>> LookForNearbyPlacesAsync(PlacesQueryTransporter query);
    }
}
