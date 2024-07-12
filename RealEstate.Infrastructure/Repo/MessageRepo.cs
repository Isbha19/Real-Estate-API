using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request.Chat;
using RealEstate.Application.DTOs.Response.Chat;
using RealEstate.Application.Helpers;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repo
{
    public class MessageRepo : IMessage
    {
        private readonly AppDbContext context;
        private readonly GetUserHelper getUserHelper;

        public MessageRepo(AppDbContext context,GetUserHelper getUserHelper)
        {
            this.context = context;
            this.getUserHelper = getUserHelper;
        }
        public void AddMessage(Message message)
        {
            context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int messageId)
        {
            return await context.Messages.FindAsync(messageId);
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var user = await getUserHelper.GetUser();
            messageParams.UserId= user.Id;
            var query = context.Messages.OrderByDescending(m => m.SentAt).AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.Receiver.Id == messageParams.UserId),
                "Outbox" => query.Where(u => u.SenderId == messageParams.UserId),
                _ => query.Where(u => u.Receiver.Id == messageParams.UserId && u.DateRead == null)
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
            var user =await getUserHelper.GetUser();
            var currentUserId=user.Id;
            var messages = await context.Messages
        .Include(u => u.Sender)
        .Include(u => u.Receiver)
        .Where(m => (m.Receiver.Id == currentUserId && m.Sender.Id == recipientId)
                    || (m.Receiver.Id == recipientId && m.Sender.Id == currentUserId))
        .OrderBy(m => m.SentAt)
        .ToListAsync();

            var unreadMessages = messages.Where(m => m.DateRead == null && m.Receiver.Id == currentUserId).ToList();
            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.Now;
                }
                await context.SaveChangesAsync();
            }

            var messageDtos = messages.Select(m => new MessageDto
            {
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

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
