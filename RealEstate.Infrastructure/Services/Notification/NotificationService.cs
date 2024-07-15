using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Response.Notification;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.SignalR;
using System.Threading;

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
            var newNotification = new NotificationDto
            {
                NotificationId = notification.Id,
                Message = notification.Message,
                Url = notification.Url,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
         
            await hubContext.Clients.User(userId).SendAsync("ReceiveNotification",newNotification);

        }
        public async Task StoreOfflineNotificationAsync(string userId, string message, string url)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Url = url,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
            };
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();
        }

    }
}
