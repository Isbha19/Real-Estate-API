using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs.Response;
using RealEstate.Application.DTOs.Response.Notification;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Contracts
{
    public interface INotification
    {
        Task<List<NotificationDto>> GetUserNotifications();
        Task<GeneralResponse> MarkAsRead(int id);
        Task<GeneralResponse> MarkNotificationsAsOpened();
        Task<int> GetUnopenedNotificationsCountAsync();


    }
}
