using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Response.Notification;
using RealEstate.Infrastructure.Services;
using System.Security.Claims;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class NotificationController : ControllerBase
    {
        private readonly INotification notification;

        public NotificationController(INotification notification)
        {
            this.notification = notification;
        }

        [HttpGet("get-user-notifications")]
        public async Task<IActionResult> GetUserNotifications()
        {
            var result = await notification.GetUserNotifications();
            return Ok(result);
        }

        [HttpPost("mark-as-read/{notificationId}")]

        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            var result = await notification.MarkAsRead(notificationId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("markAsOpened")]
        public async Task<IActionResult> MarkNotificationsAsOpened()
        {
            var result = await notification.MarkNotificationsAsOpened();
            return Ok(result);
        }
        [HttpGet("unopened-count")]
        public async Task<IActionResult> GetUnopenedNotificationsCount()
        {
             var count = await notification.GetUnopenedNotificationsCountAsync();
                return Ok(count);
           
        }
    }
}
