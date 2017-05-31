using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhereToMeet.Transporters;
using WhereToMeet.Algorithm;
using WhereToMeet.Services;
using WhereToMeet.Services.PlacesProviders;
using WhereToMeet.Database;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WhereToMeet.Controllers
{
    [Route("api/[controller]")]
    public class PerfectPlaceController : Controller
    {
        public PerfectPlaceAlgorithm Algo { get; set; }
        public IDistanceResolver DistanceResolver { get; protected set; }
        public IPlacesProvider PlacesProvider { get; protected set; }
        public ProgramDbContext DbContext { get; private set; }

        public PerfectPlaceController(IDistanceResolver distanceResolver, IPlacesProvider placesProvider, ProgramDbContext dbContext)
        {
            this.DistanceResolver = distanceResolver;
            this.PlacesProvider = placesProvider;
            this.Algo = new PerfectPlaceAlgorithm();
            this.DbContext = dbContext;
        }
        public class PerfectPlaceQueryWrapper
        {
            public string[] Types { get; set; }
            public int[] Participants { get; set; }
        }
        // GET: api/values

        IEnumerable<GeoCoordinatesTransporter> GetParticipantsCoordinates(int[] participantsIds)
        {
            var friendsGeoData = participantsIds.Select(id => this.DbContext.Users.Where(user => user.Id == id)
            .Select(user => new GeoCoordinatesTransporter() {
                X = user.LastKnownLongitude, Y = user.LastKnownLatitude
            }).FirstOrDefault());
            return friendsGeoData;
        }
        [HttpGet]
        public async Task<IActionResult> Get(PerfectPlaceQueryWrapper perfectPlaceQuery)
        {
            var participantsGeoCoordinates = this.GetParticipantsCoordinates(perfectPlaceQuery.Participants);
            if (participantsGeoCoordinates == null || !participantsGeoCoordinates.Any() || participantsGeoCoordinates.Contains(null))
                return BadRequest("Bad request. Some of the user Ids provided do not correspond to anybody in the Database.");
            var perfectPlace = await this.Algo.FindPerfectPlace(this.PlacesProvider, perfectPlaceQuery.Types,
                this.DistanceResolver, participantsGeoCoordinates.ToArray());
            return Ok(perfectPlace);
        }
    }
}
