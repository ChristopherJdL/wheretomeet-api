using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        public IActionResult Get(PerfectPlaceQueryWrapper perfectPlaceQuery)
        {
            //Do Algorithm Stuff
            return Ok();
        }
    }
}
