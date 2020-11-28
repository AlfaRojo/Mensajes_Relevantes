using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MRDB
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string name, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", name, message);
        }
    }
}
