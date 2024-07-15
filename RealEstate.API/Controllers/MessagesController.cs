using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request.Chat;
using RealEstate.Application.DTOs.Response.Chat;
using RealEstate.Application.Helpers;
using RealEstate.Domain.Entities;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessage messageRepo;
        private readonly GetUserHelper getuserHelper;
        private readonly UserManager<User> userManager;

        public MessagesController(IMessage messageRepo,GetUserHelper getuserHelper, UserManager<User> userManager)
        {
            this.messageRepo = messageRepo;
            this.getuserHelper = getuserHelper;
            this.userManager = userManager;
        }
        [HttpPost("send-message")]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var user =await getuserHelper.GetUser();
            if (user.Id == createMessageDto.RecipientId.ToLower())
            {
                return BadRequest("You cannot send messages to yourself");
            }
            var sender = user;
            var recipient= await userManager.FindByIdAsync(createMessageDto.RecipientId);
            if(recipient == null)
            {
                return NotFound();
            }
            var message = new Message
            {
                Sender = sender,
                Receiver = recipient,
                Content = createMessageDto.Content

            };
            messageRepo.AddMessage(message);
            if(await messageRepo.SaveAllAsync())
            {
                var messageDto = new MessageDto
                {
                    SenderId = message.Sender.Id,
                    SenderName = $"{sender.FirstName} {sender.LastName}",
                    ReceiverId = recipient.Id,
                    ReceiverName = $"{recipient.FirstName} {recipient.LastName}",
                    Content = message.Content,
                    SentAt = message.SentAt,
                };

                return Ok(messageDto);
            }
          return BadRequest();
        }
        [HttpGet("get-messages-for-user")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageForUser([FromQuery]MessageParams messageParams)
        {
            var messages = await messageRepo.GetMessagesForUser(messageParams);
            return Ok(messages);
        }
        [HttpGet("get-chats-for-user")]
        public async Task<ActionResult<IEnumerable<UserChatsDto>>> GetChatsForUser()
        {
            var chats = await messageRepo.GetChatUsersAsync();
            return Ok(chats);
        }
        [HttpGet("thread/{recipientId}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string recipientId)
        {
            return Ok(await messageRepo.GetMessageThread(recipientId));
        }
        [HttpDelete("delete-message/{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            var user =await getuserHelper.GetUser();
            string userId=user.Id;
            var message=await messageRepo.GetMessage(messageId);
            if(message.Sender.Id!=userId&& message.Receiver.Id != userId)
            {
                return Unauthorized();
            }

            if (message.Sender.Id == userId)
            {
                message.SenderDeleted = true;
            }
            if(message.Receiver.Id == userId)
            {
                message.RecipientDeleted = true;
            }
            if(message.SenderDeleted&& message.RecipientDeleted)
            {
                messageRepo.DeleteMessage(message);
            }
            if(await messageRepo.SaveAllAsync()) { return Ok(); }
            return BadRequest("Problem deleting the message");
        }
    }
}
