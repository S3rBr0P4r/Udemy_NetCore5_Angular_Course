using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public UsersController(IAppUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] AppUserEditRequest request)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userResponse = await _repository.GetUserByUserNameAsync(userName).ConfigureAwait(false);
            var user = await _repository.GetUserByIdAsync(userResponse.Id).ConfigureAwait(false);

            _mapper.Map(request, user);

            _repository.Update(user);

            return await _repository.SaveAllChangesAsync().ConfigureAwait(false)
                ? (ActionResult) NoContent()
                : BadRequest($"Failed to update the user '{userName}'");
        }
    }
}