using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;


namespace RealEstate.Infrastructure.Services
{
  
        public class ChatHub : Hub
        {
        private readonly AppDbContext _dbContext;

        public ChatHub(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            var newMessage = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = message
            };

            _dbContext.Messages.Add(newMessage);
            await _dbContext.SaveChangesAsync();

            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);

            // Notify the receiver (agent) that they have a new message
            await Clients.User(receiverId).SendAsync("ReceiveMessageNotification", senderId);
        }
    }
    }
