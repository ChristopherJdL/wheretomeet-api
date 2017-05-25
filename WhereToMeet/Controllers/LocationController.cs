using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhereToMeet.Database;
using WhereToMeet.Extensions;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WhereToMeet.Controllers
{
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        public ProgramDbContext DbContext { get; private set; }

        public LocationController(ProgramDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public class GeoCoordinatesInputWrapper
        {
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put(GeoCoordinatesInputWrapper geoCoordInput)
        {
            if (geoCoordInput.Latitude == null || geoCoordInput.Longitude == null)
                return BadRequest("Badly formatted new position.");
            var user = DbContext.Users.Where(u => u.Id == this.User.GetUserId()).First();
            user.LastKnownLatitude = (double)geoCoordInput.Latitude;
            user.LastKnownLongitude = (double)geoCoordInput.Longitude;
            DbContext.Update(user);
            DbContext.SaveChanges();
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
