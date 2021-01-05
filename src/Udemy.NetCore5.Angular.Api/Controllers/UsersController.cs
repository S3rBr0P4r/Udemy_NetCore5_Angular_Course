using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.DTOs;
using Udemy.NetCore5.Angular.Logic.Extensions;
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
        private readonly IPhotoService _photoService;

        public UsersController(IAppUserRepository repository, IMapper mapper, IPhotoService photoService)
        {
            _repository = repository;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IEnumerable<AppUserResponse>> GetUsers()
        {
            return await _repository.GetUsersAsync().ConfigureAwait(false);
        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<AppUserResponse> GetUser(string userName)
        {
            return await _repository.GetUserByUserNameAsync(userName).ConfigureAwait(false);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] AppUserEditRequest request)
        {
            var userName = User.GetUserName();
            var userResponse = await _repository.GetUserByUserNameAsync(userName).ConfigureAwait(false);
            var user = await _repository.GetUserByIdAsync(userResponse.Id).ConfigureAwait(false);

            _mapper.Map(request, user);

            _repository.Update(user);

            return await _repository.SaveAllChangesAsync().ConfigureAwait(false)
                ? (ActionResult) NoContent()
                : BadRequest($"Failed to update the user '{userName}'");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<AppUserPhotosResponse>> AddPhoto(IFormFile file)
        {
            var userName = User.GetUserName();
            var userResponse = await _repository.GetUserByUserNameAsync(userName).ConfigureAwait(false);
            var user = await _repository.GetUserByIdAsync(userResponse.Id).ConfigureAwait(false);

            var result = await _photoService.AddPhotoAsync(file).ConfigureAwait(false);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count.Equals(0))
            {
                photo.Enabled = true;
            }

            user.Photos.Add(photo);

            if (await _repository.SaveAllChangesAsync())
            {
                return CreatedAtRoute("GetUser", new { user.UserName }, _mapper.Map<AppUserPhotosResponse>(photo));
            }

            return BadRequest("Problem adding photo");
        }
    }
}