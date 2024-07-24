using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request.Chat;
using RealEstate.Application.DTOs.Response.Chat;
using RealEstate.Application.Helpers;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.signalr;
using RealEstate.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IMessage messageRepo;
        private readonly GetUserHelper getUser;
        private readonly UserManager<User> userManager;
        private readonly IHubContext<PresenceHub> presenceHub;
        private readonly PresenceTracker tracker;
        private readonly NotificationService notificationService;

        public MessageHub(IMessage messageRepo, GetUserHelper getUser,
            UserManager<User> userManager,IHubContext<PresenceHub> presenceHub,
            PresenceTracker tracker,NotificationService notificationService)
        {
            this.messageRepo = messageRepo;
            this.getUser = getUser;
            this.userManager = userManager;
            this.presenceHub = presenceHub;
            this.tracker = tracker;
            this.notificationService = notificationService;
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var user = await getUser.GetUser();
            string userId = user.Id;
            var groupName = GetGroupName(userId, otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group=await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);
            var messages = await messageRepo.GetMessageThread(otherUser);
            await Clients.Caller.SendAsync("RecieveMessageThread", messages);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group=await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var user = await getUser.GetUser();
            if (user.Id == createMessageDto.RecipientId.ToLower())
            {
                throw new HubException("You cannot send messages to yourself");
            }
            var sender = user;
            var recipient = await userManager.FindByIdAsync(createMessageDto.RecipientId);
            if (recipient == null)
            {
                throw new HubException("User not found");
            }
            var message = new Message
            {
                Sender = sender,
                Receiver = recipient,
                Content = createMessageDto.Content,
            };
            var groupName = GetGroupName(sender.Id, recipient.Id);
            var group = await messageRepo.GetGroup(groupName);
            if (group.Connections.Any(x => x.UserId == recipient.Id))
            {
                // The recipient has the chat box open
                message.DateRead = DateTimeOffset.UtcNow;
            }
            else
            {
                var connections = await tracker.GetConnectionsForUser(recipient.Id);

                var notificationMessage = $"You have received a new message from {sender.FirstName} {sender.LastName}";
                var notificationUrl = "/messages"; // Adjust URL as per your application logic

                if (connections != null)
                {
                   
                    // Send notification to recipient if they are online but chat box is not open
                    await notificationService.NotifyUserAsync(recipient.Id, notificationMessage, notificationUrl);
                }
                else
                {
                    // Store notification for offline users
                    await notificationService.StoreOfflineNotificationAsync(recipient.Id, notificationMessage, notificationUrl);
                }
            }
            messageRepo.AddMessage(message);
            if (await messageRepo.SaveAllAsync())
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

                await Clients.Group(groupName).SendAsync("NewMessage", messageDto);
            }
        }
        public async Task SendTypingNotification(string recipientId)
      {
            var user = await getUser.GetUser();
            var recipient = await userManager.FindByIdAsync(recipientId);
            if (recipient == null)
            {
                throw new HubException("Recipient not found");
            }

            var groupName = GetGroupName(user.Id, recipient.Id);
            await Clients.Group(groupName).SendAsync("UserTyping", user.Id);
        }

        public async Task SendStoppedTypingNotification(string recipientId)
        {
            var user = await getUser.GetUser();
            var recipient = await userManager.FindByIdAsync(recipientId);
            if (recipient == null)
            {
                throw new HubException("Recipient not found");
            }

            var groupName = GetGroupName(user.Id, recipient.Id);
            await Clients.Group(groupName).SendAsync("UserStoppedTyping", user.Id);
        }
   
        private async Task<Group> AddToGroup(string groupName)
        {

            var user = await getUser.GetUser();

            var group = await messageRepo.GetGroup(groupName);
            var connection = new Connection(Context.ConnectionId, user.Id);
            if (group == null)
            {
                group = new Group(groupName);
                messageRepo.AddGroup(group);
            }
            group.Connections.Add(connection);
            if(await messageRepo.SaveAllAsync()) return group;
            throw new HubException("Failed to join the group");
        }
        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await messageRepo.GetGroupForConnection(Context.ConnectionId);
            var connection=group.Connections.FirstOrDefault(x=>x.ConnectionId==Context.ConnectionId);
            messageRepo.RemoveConnection(connection);
           if( await messageRepo.SaveAllAsync())return group;
            throw new HubException("Failed to remove from the group");
        }
        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}
