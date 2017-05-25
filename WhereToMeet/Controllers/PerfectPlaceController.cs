using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhereToMeet.Transporters;
using WhereToMeet.Algorithm;
using WhereToMeet.Services;
using WhereToMeet.Services.PlacesProviders;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WhereToMeet.Controllers
{
    [Route("api/[controller]")]
    public class PerfectPlaceController : Controller
    {
        public PerfectPlaceAlgorithm Algo { get; set; }
        public IDistanceResolver DistanceResolver { get; protected set; }
        public IPlacesProvider PlacesProvider { get; protected set; }

        public PerfectPlaceController(IDistanceResolver distanceResolver, IPlacesProvider placesProvider)
        {
            this.DistanceResolver = distanceResolver;
            this.PlacesProvider = placesProvider;
            this.Algo = new PerfectPlaceAlgorithm();
        }
        public class PerfectPlaceQueryWrapper
        {
            public string[] Types { get; set; }
            public int[] Participants { get; set; }
        }
        // GET: api/values

        GeoCoordinatesTransporter[] GetParticipantsCoordinates(int[] ParticipantsIds)
        {
            return new GeoCoordinatesTransporter [] 
            {
                new GeoCoordinatesTransporter()
                {
                    Y =  37.532600,
                    X = 127.024612
                },
                new GeoCoordinatesTransporter()
                {
                    Y = 37.5588440,
                    X = 126.9198570
                },

                new GeoCoordinatesTransporter()
                {
                    Y = 37.5593203,
                    X = 126.9229898
                }
            };
        }
        [HttpGet]
        public async Task<IActionResult> Get(PerfectPlaceQueryWrapper perfectPlaceQuery)
        {
            var perfectPlace = await this.Algo.FindPerfectPlace(this.PlacesProvider, perfectPlaceQuery.Types,
                this.DistanceResolver, this.GetParticipantsCoordinates(perfectPlaceQuery.Participants));
            return Ok(perfectPlace);
        }
    }
}
