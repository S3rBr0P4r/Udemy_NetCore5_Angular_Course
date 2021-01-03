using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.NetCore5.Angular.Data;
using Udemy.NetCore5.Angular.Data.Entities;

namespace Udemy.NetCore5.Angular.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuggyController : ControllerBase
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("auth")]
        [Authorize]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<string> GetNotFound()
        {
            var thing = _context.Users.Find(-1);

            return thing == null ? (ActionResult<string>) NotFound() : Ok(thing);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Users.Find(-1);

            var thingToReturn = thing.ToString();

            return thingToReturn;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }
    }
}