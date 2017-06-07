using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhereToMeet.Database;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WhereToMeet.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        //TODO: Doc in markdown.
        protected ProgramDbContext dbContext;
        public RegisterController(ProgramDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public class RegisterDataWrapper
        {
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
        [HttpPost]
        public IActionResult Post(RegisterDataWrapper rdw)
        {

            if (rdw == null || string.IsNullOrEmpty(rdw.Password) ||
                string.IsNullOrEmpty(rdw.Email) || string.IsNullOrEmpty(rdw.Username))
                return BadRequest("Information for registration is invalid.");
            if (!this.dbContext.Users.Where(u => u.Email == rdw.Email).Any())
            {
                dbContext.Add<User>(new User
                {
                    Email = rdw.Email,
                    Password = rdw.Password,
                    Username = rdw.Username,
                });
                dbContext.SaveChanges();
                return Ok();
            }
            else
                return BadRequest("User already added.");
        }
    }
}
