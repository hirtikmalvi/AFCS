using Microsoft.AspNetCore.SignalR;

namespace AFCS.API.Hubs
{
    public class FareHub: Hub
    {
        public static readonly string NewTransaction = "NewTransaction";
        public static readonly string GateStatusChanged = "GateStatusChanged";

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(ex);
        }
    }
}
