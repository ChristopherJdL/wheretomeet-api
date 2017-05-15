using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhereToMeet.Transporters;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WhereToMeet.Controllers
{
    [Route("api/[controller]")]
    public class PerfectPlaceController : Controller
    {
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
                    X =  126.9960697,
                    Y = 37.5156865
                },
                new GeoCoordinatesTransporter()
                {
                    X = 145.9960697,
                    Y = 40.51004
                },

                new GeoCoordinatesTransporter()
                {
                    X = 150.9960697,
                    Y = 39.51004
                }
            };
        }
        [HttpGet]
        public IActionResult Get(PerfectPlaceQueryWrapper perfectPlaceQuery)
        {
            //Do Algorithm Stuff
            return Ok();
        }
    }
}
