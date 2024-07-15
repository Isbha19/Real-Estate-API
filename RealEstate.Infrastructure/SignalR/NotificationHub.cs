using Microsoft.AspNetCore.SignalR;
using RealEstate.Application.Contracts;
using RealEstate.Application.Helpers;
using RealEstate.Infrastructure.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.SignalR
{
    public class NotificationHub :Hub
    {
        private readonly GetUserHelper userhelper;
        private readonly INotification notificationRepo;

        public NotificationHub(GetUserHelper userhelper,INotification notificationRepo)
        {
            this.userhelper = userhelper;
            this.notificationRepo = notificationRepo;
        }
        public override async Task OnConnectedAsync()
        {
            var user = await userhelper.GetUser();

            var userId = user.Id;
            // Log the user ID to ensure it matches
            Console.WriteLine($"User connected: {userId}");
            var messages = await notificationRepo.GetUserNotifications();
            await Clients.Caller.SendAsync("AllNotification", messages);
            await base.OnConnectedAsync();
        }
    }
}
