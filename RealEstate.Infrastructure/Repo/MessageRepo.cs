using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Account;
using RealEstate.Application.DTOs.Request.Chat;
using RealEstate.Application.DTOs.Response.Chat;
using RealEstate.Application.Helpers;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.signalr;
using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.SignalR;

namespace RealEstate.Infrastructure.Repo
{
    public class MessageRepo : IMessage
    {
        private readonly AppDbContext context;
        private readonly GetUserHelper getUserHelper;
        private readonly UserManager<User> userManager;
        private readonly IHubContext<MessageHub> hubContext;

        public MessageRepo(AppDbContext context, GetUserHelper getUserHelper, UserManager<User> userManager, IHubContext<MessageHub> hubContext)
        {
            this.context = context;
            this.getUserHelper = getUserHelper;
            this.userManager = userManager;
            this.hubContext = hubContext;
        }

        public void AddGroup(Group group)
        {
            context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroup(string groupName)
        {
            if (groupName == null)
            {
                throw new ArgumentNullException(nameof(groupName));
            }

            return await context.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await context.Groups
                            .Include(x => x.Connections)
                            .Where(c => c.Connections.Any(x => x.ConnectionId == connectionId))
                            .FirstOrDefaultAsync();
        }

        public async Task<Message> GetMessage(int messageId)
        {
            return await context.Messages
                .Include(u => u.Sender)
                .Include(u => u.Receiver)
                .SingleOrDefaultAsync(x => x.Id == messageId);
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var user = await getUserHelper.GetUser();
            messageParams.UserId = user.Id;
            var query = context.Messages.OrderByDescending(m => m.SentAt).AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.Receiver.Id == messageParams.UserId && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderId == messageParams.UserId && u.SenderDeleted == false),
                _ => query.Where(u => u.Receiver.Id == messageParams.UserId && u.RecipientDeleted == false && u.DateRead == null)
            };
            var messages = await query.Select(m => new MessageDto
            {
                SenderId = m.Sender.Id,
                SenderName = m.Sender.FirstName + " " + m.Sender.LastName,
                ReceiverId = m.Receiver.Id,
                ReceiverName = m.Receiver.FirstName + " " + m.Receiver.LastName,
                Content = m.Content,
                DateRead = m.DateRead,
                SentAt = m.SentAt
            }).ToListAsync();

            return messages;

        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string recipientId)
        {
            var user = await getUserHelper.GetUser();
            var currentUserId = user.Id;
            var messages = await context.Messages
        .Include(u => u.Sender)
        .Include(u => u.Receiver)
        .Where(m => m.Receiver.Id == currentUserId && m.RecipientDeleted == false && m.Sender.Id == recipientId
                    || (m.Receiver.Id == recipientId && m.Sender.Id == currentUserId && m.SenderDeleted == false))
        .OrderBy(m => m.SentAt)
        .ToListAsync();

            var unreadMessages = messages.Where(m => m.DateRead == null && m.Receiver.Id == currentUserId).ToList();
            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;

                }
                await context.SaveChangesAsync();
                foreach (var message in unreadMessages)
                {
                    await hubContext.Clients.User(recipientId).SendAsync("MessageRead", message.Id);
                }
            }

            var messageDtos = messages.Select(m => new MessageDto
            {
                messageId=m.Id,
                SenderId = m.Sender.Id,
                SenderName = m.Sender.FirstName + " " + m.Sender.LastName,
                ReceiverId = m.Receiver.Id,
                ReceiverName = m.Receiver.FirstName + " " + m.Receiver.LastName,
                Content = m.Content,
                DateRead = m.DateRead,
                SentAt = m.SentAt
            });

            return messageDtos;
        }


        public async Task<IEnumerable<UserChatsDto>> GetChatUsersAsync()
        {
            var user = await getUserHelper.GetUser();
            string userId = user.Id;
            var messages = await context.Messages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
               
                .Select(m => new
                {
                    m.Id,
                    Sender = context.Users.FirstOrDefault(u => u.Id == m.SenderId),
                    Receiver = context.Users.FirstOrDefault(u => u.Id == m.ReceiverId)
                })
                .ToListAsync();

            var chatUsers = messages
                .Select(m => m.Sender == user ? m.Receiver : m.Sender)
                .Distinct()
                .ToList();

            var userDtos = new List<UserChatsDto>();

            foreach (var chatUser in chatUsers)
            {
                var roles = await userManager.GetRolesAsync(chatUser);

                string imageUrl = "https://static.vecteezy.com/system/resources/previews/008/442/086/non_2x/illustration-of-human-icon-user-symbol-icon-modern-design-on-blank-background-free-vector.jpg"; // default image

                if (roles.Contains("Agent"))
                {
                    imageUrl = chatUser?.Agent?.ImageUrl.ImageUrl ?? imageUrl;
                }
                else if (roles.Contains("Company"))
                {
                    imageUrl = chatUser.company.CompanyLogo.ImageUrl ?? imageUrl;
                }

                userDtos.Add(new UserChatsDto
                {
                    UserId = chatUser.Id,
                    UserName = chatUser.FirstName + " " + chatUser.LastName,
                    ImageUrl = imageUrl
                });
            }

            return userDtos;
        }


        public void RemoveConnection(Connection connection)
        {
            context.Connections.Remove(connection);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
