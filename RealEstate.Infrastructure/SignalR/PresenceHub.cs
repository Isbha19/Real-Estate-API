using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RealEstate.Application.Helpers;



namespace RealEstate.Infrastructure.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly GetUserHelper getUser;
        private readonly PresenceTracker tracker;

        public PresenceHub(GetUserHelper getUser,PresenceTracker tracker)
        {
            this.getUser = getUser;
            this.tracker = tracker;
        }
        public override async Task OnConnectedAsync()
        {
            var user = await getUser.GetUser();
            var userId = user.Id;
           var isOnline=await tracker.UserConnected(userId, Context.ConnectionId);
            if(isOnline)
            {
                await Clients.Others.SendAsync("UserIsOnline", userId);

            }

            var currentUsers=await tracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await getUser.GetUser();
            var userId = user.Id;
            var isOffline=await tracker.UserDisconnected(userId, Context.ConnectionId);
            if(isOffline)
            {
                await Clients.Others.SendAsync("UserIsOffline", userId);

            }

            await base.OnDisconnectedAsync(exception);

        }
    }
}
