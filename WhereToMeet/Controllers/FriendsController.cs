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

        protected IEnumerable<dynamic> ObtainFriends()
        {
            var friendsOfThisUser = this.dbContext.Friendships
                .Where(f => f.UserId == this.User.GetUserId()).Include(f => f.Friend)
                .Select(u => new
                {
                    Id = u.Friend.Id,
                    Username = u.Friend.Username,
                    Email = u.Friend.Email,
                    lastKnownY = u.Friend.LastKnownLatitude,
                    lastKnownX = u.Friend.LastKnownLongitude
                });
            return friendsOfThisUser;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return new OkObjectResult(this.ObtainFriends());
        }

        protected bool IsFriendAlreadyAdded(User friendToVerify)
        {
            var resultsSet = this.dbContext.Friendships
               .Where(f => f.UserId == this.User.GetUserId() && f.FriendId == friendToVerify.Id);
            return resultsSet.Any();
        }

        protected void AddFriend(User friendToAdd)
        {

            this.dbContext.Friendships.Add(new Friendship()
            {
                FriendId = friendToAdd.Id,
                UserId = this.User.GetUserId()
            });
            this.dbContext.SaveChanges();
        }

        // POST: Add Friend by Username
        [HttpPost]
        public IActionResult Post(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Username is empty.");
            var usersHavingUsername = this.dbContext.Users.Where(u => u.Username == username);
            if (!usersHavingUsername.Any())
                return NoContent();
            var friendToAdd = usersHavingUsername.First();
            if (this.IsFriendAlreadyAdded(friendToAdd))
                return BadRequest("Friend is already added.");
            this.AddFriend(friendToAdd);
            return Ok(this.ObtainFriends());
        }
    }
}
