using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.SignalR;

namespace RealEstate.Infrastructure.Services
{
    public class NotificationService
    {
        private readonly AppDbContext context;
        private readonly IHubContext<NotificationHub> hubContext;

        public NotificationService(AppDbContext context, IHubContext<NotificationHub> hubContext)
        {
            this.context = context;
            this.hubContext = hubContext;
        }
        public async Task NotifyUserAsync(string userId, string message, string url)
        {
            var notification = new Domain.Entities.Notification
            {
                UserId = userId,
                Message = message,
                Url = url,
                IsRead = false
            };

            context.Notifications.Add(notification);
            await context.SaveChangesAsync();
            await hubContext.Clients.User(userId).SendAsync("ReceiveNotification", new
            {
                Message = message,
                Url = url
            });

        }
    }
}
