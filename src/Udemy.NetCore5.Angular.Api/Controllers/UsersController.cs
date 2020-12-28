using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Udemy.NetCore5.Angular.Data;
using Udemy.NetCore5.Angular.Data.Entities;

namespace Udemy.NetCore5.Angular.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await _context.Users.ToListAsync().ConfigureAwait(false);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<AppUser> GetUser(int id)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
        }
    }
}