using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using BT.BrightMarket.WebAPI.Hubs;
using Microsoft.AspNetCore.Http;

namespace BT.BrightMarket.WebAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ConnectedClientsService connectedClientsService;

        public ChatHub(ConnectedClientsService connectedClientsService)
        {
            this.connectedClientsService = connectedClientsService;
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                var userIdString = Context.GetHttpContext().Request.Query["userId"];
                if (int.TryParse(userIdString, out int userId))
                {
                    connectedClientsService.AddMessageSessionId(userId, Context.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                // Handle exception, if needed
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            connectedClientsService.RemoveMessageSessionId(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
