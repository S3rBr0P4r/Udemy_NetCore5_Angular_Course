using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Udemy.NetCore5.Angular.Api.DTOs;
using Udemy.NetCore5.Angular.Data;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.Interfaces;

namespace Udemy.NetCore5.Angular.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserTokenResponse>> Register([FromBody] RegisterUserRequest request)
        {
            if (await UserExists(request.UserName).ConfigureAwait(false))
            {
                return BadRequest("Username is taken");
            }

            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = request.UserName.ToLowerInvariant(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
                PasswordSalt = hmac.Key
            };

            await _context.Users.AddAsync(user).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return new UserTokenResponse
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserTokenResponse>> Login([FromBody] LoginUserRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == request.UserName.ToLowerInvariant()).ConfigureAwait(false);

            if (user == null)
            {
                return Unauthorized("Invalid username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var requestPasswordHashed = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

            if (requestPasswordHashed.Where((t, i) => t != user.PasswordHash[i]).Any())
            {
                return Unauthorized("Invalid password");
            }

            return new UserTokenResponse
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string userName)
        {
            return await _context.Users.AnyAsync(u => u.UserName == userName.ToLowerInvariant()).ConfigureAwait(false);
        }
    }
}