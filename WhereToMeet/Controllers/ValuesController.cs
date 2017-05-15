using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhereToMeet.Database;
using Microsoft.AspNetCore.Authorization;

namespace WhereToMeet.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            var dbContext = new ProgramDbContext();
            
            dbContext.SaveChanges();
            return dbContext.Users.Select(u => u.Username);
            
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public int Get(int id)
        {
            var dbContext = new ProgramDbContext();
            
            return dbContext.Users.First().Id;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
