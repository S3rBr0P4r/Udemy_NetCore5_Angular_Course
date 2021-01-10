using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.DTOs;
using Udemy.NetCore5.Angular.Logic.Helpers;
using Udemy.NetCore5.Angular.Logic.Interfaces;

namespace Udemy.NetCore5.Angular.Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserTokenResponse>> Register([FromBody] RegisterUserRequest request)
        {
            var user = await GetUser(request.UserName).ConfigureAwait(false);

            if (user != null)
            {
                return BadRequest("Username is taken");
            }

            var newUser = _mapper.Map<AppUser>(request);

            newUser.UserName = request.UserName.ToLowerInvariant();

            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return new UserTokenResponse
            {
                UserName = newUser.UserName,
                Token = _tokenService.CreateToken(newUser),
                KnownAs = newUser.KnownAs,
                Gender = newUser.Gender
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserTokenResponse>> Login([FromBody] LoginUserRequest request)
        {
            var user = await GetUser(request.UserName).ConfigureAwait(false);

            if (user == null)
            {
                return Unauthorized("Invalid username");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            
            return new UserTokenResponse
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.Enabled)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        private async Task<AppUser> GetUser(string userName)
        {
            return await _userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(u => u.UserName == userName.ToLowerInvariant()).ConfigureAwait(false);
        }
    }
}