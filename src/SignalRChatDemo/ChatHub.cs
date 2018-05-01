using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SignalRChatDemo
{
   [Authorize(AuthenticationSchemes = "Identity.Application" + "," + JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly PresenceTracker presence;

        public ChatHub(PresenceTracker presence)
        {
            this.presence = presence;
        }

        public Task SendMessage(string message) 
        {
            return Clients.All.SendCoreAsync("newMessage", new object[] { Context.User.Identity.Name, message });
        }

        public override async Task OnConnectedAsync()
        {
            string username = Context.User?.Identity?.Name;
            if (username != null)
            {
                var result = await presence.ConnectionOpened(username);
                if (result.UserJoined)
                {
                    await Clients.All.SendAsync("userJoined", username);
                }
            }
            Console.WriteLine($"{username ?? "anonymous"} joined");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string username = Context.User?.Identity?.Name;
            if (username != null)
            {
                var result = await presence.ConnectionClosed(username);
                if (result.UserLeft)
                {
                    await Clients.All.SendAsync("userLeft", username);
                }
            }
            Console.WriteLine($"{username ?? "anonymous"} left");
            await base.OnDisconnectedAsync(exception);
        }
    }
}