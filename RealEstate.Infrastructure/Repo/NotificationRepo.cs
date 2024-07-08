using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Response;
using RealEstate.Application.DTOs.Response.Notification;
using RealEstate.Application.Helpers;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Repo
{
    public class NotificationRepo : INotification
    {
        private readonly AppDbContext context;
        private readonly GetUserHelper userAuth;

        public NotificationRepo(AppDbContext context, GetUserHelper userAuth)
        {
            this.context = context;
            this.userAuth = userAuth;
        }
        public async Task<List<NotificationDto>> GetUserNotifications()
        {
            var user = await userAuth.GetUser();
            string userId = user.Id;

            var notifications = await context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

            var notificationDtos = notifications.Select(n => new NotificationDto
            {
                NotificationId=n.Id,
                Message = n.Message,
                Url = n.Url,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            }).ToList();

            return notificationDtos;
        }

        public async Task<GeneralResponse> MarkAsRead(int notificationId)
        {
            var notification = await context.Notifications.FindAsync(notificationId);
            if (notification == null) return new GeneralResponse(false, "notification not found");

            notification.IsRead = true;
            await context.SaveChangesAsync();

            return new GeneralResponse(true, "marked as read");
        }
        public async Task<GeneralResponse> MarkNotificationsAsOpened()
        {
            var user = await userAuth.GetUser();
            string userId = user.Id;
            if (userId == null)
            {
                return new GeneralResponse(false, "user not found");
            }
            var notifications = await context.Notifications
       .Where(n => n.UserId == userId && !n.IsOpened)
       .ToListAsync();
            foreach (var notification in notifications)
            {
                notification.IsOpened = true;
            }

            await context.SaveChangesAsync();

            return new GeneralResponse(true, "Notifications marked as opened");
        }
        public async Task<int> GetUnopenedNotificationsCountAsync()
        {
            var user = await userAuth.GetUser();
            if (user == null)
            {
                throw new ApplicationException("User not found");
            }

            string userId = user.Id;
            var count = await context.Notifications
                .Where(n => n.UserId == userId && !n.IsOpened)
                .CountAsync();

            return count;
        }
    }
}
