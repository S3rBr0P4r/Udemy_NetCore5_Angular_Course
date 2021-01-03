using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.NetCore5.Angular.Logic.DTOs;
using Udemy.NetCore5.Angular.Logic.Interfaces;

namespace Udemy.NetCore5.Angular.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IAppUserRepository _repository;

        public UsersController(IAppUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<AppUserResponse>> GetUsers()
        {
            return await _repository.GetUsersAsync().ConfigureAwait(false);
        }

        [HttpGet("{username}")]
        public async Task<AppUserResponse> GetUser(string userName)
        {
            return await _repository.GetUserByUserNameAsync(userName).ConfigureAwait(false);
        }
    }
}