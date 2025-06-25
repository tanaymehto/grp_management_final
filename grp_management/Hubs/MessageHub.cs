using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace grp_management.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessageUpdate(object message)
        {
            await Clients.All.SendAsync("ReceiveMessageUpdate", message);
        }
    }
} 