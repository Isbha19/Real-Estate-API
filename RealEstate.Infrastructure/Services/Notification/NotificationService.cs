using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Infrastructure.Data;
using RealEstate.Domain.Entities;
namespace RealEstate.Infrastructure.Services
{
    public class NotificationService
    {
        private readonly AppDbContext context;

        public NotificationService(AppDbContext context)
        {
            this.context = context;
        }
        public async Task NotifyUserAsync(string userId, string message, string url)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Url = url,
                IsRead = false
            };

            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

        }
    }
}
