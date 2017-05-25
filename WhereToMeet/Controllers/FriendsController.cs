using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Extensions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WhereToMeet.Database;
using Microsoft.AspNetCore.Authorization;
using WhereToMeet.Extensions;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WhereToMeet.Controllers
{
    [Route("api/[controller]")]
    public class FriendsController : Controller
    {
        ProgramDbContext dbContext;
        public FriendsController(ProgramDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var friendsOfThisUser = this.dbContext.Friendships.Where(f => f.UserId == this.User.GetUserId()).Include(f => f.Friend).Select(u => u.Friend);
            return new OkObjectResult(friendsOfThisUser);
        }

        // POST Set default friends
        [HttpPost]
        public IActionResult Post()
        {
            return Ok();
        }
    }
}
