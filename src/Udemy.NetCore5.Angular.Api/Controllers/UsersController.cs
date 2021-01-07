using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.DTOs;
using Udemy.NetCore5.Angular.Logic.Extensions;
using Udemy.NetCore5.Angular.Logic.Helpers;
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
        public async Task<ActionResult<IEnumerable<AppUserResponse>>> GetUsers([FromQuery] UserParams userParams)
        {
            var users = await _repository.GetUsersAsync(userParams).ConfigureAwait(false);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<AppUserResponse> GetUser(string userName)
        {
            return await _repository.GetUserByUserNameAsync(userName).ConfigureAwait(false);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] AppUserEditRequest request)
        {
            var user = await GetUser().ConfigureAwait(false);

            _mapper.Map(request, user);

            _repository.Update(user);

            return await _repository.SaveAllChangesAsync().ConfigureAwait(false)
                ? (ActionResult) NoContent()
                : BadRequest($"Failed to update the user '{user.UserName}'");
        }
        
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await GetUser().ConfigureAwait(false);
            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo.Enabled)
            {
                return BadRequest("This is already your main photo");
            }

            var currentMain = user.Photos.FirstOrDefault(p => p.Enabled);
            if (currentMain != null)
            {
                currentMain.Enabled = false;
            }

            photo.Enabled = true;

            return await _repository.SaveAllChangesAsync()
                ? (ActionResult) NoContent()
                : BadRequest("Failed to set main photo");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<AppUserPhotosResponse>> AddPhoto(IFormFile file)
        {
            var user = await GetUser().ConfigureAwait(false);

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

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await GetUser().ConfigureAwait(false);
            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo == null)
            {
                return NotFound();
            }

            if (photo.Enabled)
            {
                return BadRequest("You cannot delete your main photo");
            }

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId).ConfigureAwait(false);
                if (result.Error != null)
                {
                    return BadRequest(result.Error.Message);
                }
            }
            
            user.Photos.Remove(photo);

            if (await _repository.SaveAllChangesAsync().ConfigureAwait(false))
            {
                return Ok();
            }

            return BadRequest("Problem deleting photo");
        }

        private async Task<AppUser> GetUser()
        {
            var userName = User.GetUserName();
            var userResponse = await _repository.GetUserByUserNameAsync(userName).ConfigureAwait(false);
            return await _repository.GetUserByIdAsync(userResponse.Id).ConfigureAwait(false);
        }
    }
}