using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.DTOs;
using Udemy.NetCore5.Angular.Logic.Extensions;
using Udemy.NetCore5.Angular.Logic.Helpers;
using Udemy.NetCore5.Angular.Logic.Interfaces;

namespace Udemy.NetCore5.Angular.Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IAppUserRepository _userRepository;
        private readonly IMessagesRepository _messagesRepository;
        private readonly IMapper _mapper;

        public MessagesController(IAppUserRepository userRepository, IMessagesRepository messagesRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _messagesRepository = messagesRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<AppUserMessagesResponse>> CreateMessage([FromBody] AppUserMessagesRequest request)
        {
            var userName = User.GetUserName();

            if (userName == request.RecipientUserName.ToLowerInvariant())
            {
                return BadRequest("You cannot send messages to yourself");
            }

            var sender = await GetUser(userName).ConfigureAwait(false);
            var recipient = await GetUser(request.RecipientUserName).ConfigureAwait(false);

            if (recipient == null)
            {
                return NotFound();
            }

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = request.Content
            };

            _messagesRepository.AddMessage(message);

            if (await _messagesRepository.SaveAllAsync())
            {
                return Ok(_mapper.Map<AppUserMessagesResponse>(message));
            }

            return BadRequest("Failed to send message");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserMessagesResponse>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.UserName = User.GetUserName();
            var messages = await _messagesRepository.GetMessagesForUser(messageParams).ConfigureAwait(false);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

            return messages;
        }

        [HttpGet("thread/{userName}")]
        public async Task<ActionResult<IEnumerable<AppUserMessagesResponse>>> GetMessagesThread(string userName)
        {
            var currentUserName = User.GetUserName();

            return Ok(await _messagesRepository.GetMessageThread(currentUserName, userName).ConfigureAwait(false));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var userName = User.GetUserName();
            var message = await _messagesRepository.GetMessage(id);

            if (message.Sender.UserName != userName && message.Recipient.UserName != userName)
            {
                return Unauthorized();
            }

            if (message.Sender.UserName == userName)
            {
                message.SenderDeleted = true;
            }

            if (message.Recipient.UserName == userName)
            {
                message.RecipientDeleted = true;
            }

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                _messagesRepository.DeleteMessage(message);
            }

            if (await _messagesRepository.SaveAllAsync().ConfigureAwait(false))
            {
                return Ok();
            }

            return BadRequest("Problem deleting the message");
        }

        private async Task<AppUser> GetUser(string userName)
        {
            var userResponse = await _userRepository.GetUserByUserNameAsync(userName).ConfigureAwait(false);
            return await _userRepository.GetUserByIdAsync(userResponse.Id).ConfigureAwait(false);
        }
    }
}