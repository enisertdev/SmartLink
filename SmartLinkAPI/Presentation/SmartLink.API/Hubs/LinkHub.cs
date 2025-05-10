using Microsoft.AspNetCore.SignalR;

namespace SmartLink.API.Hubs
{
    public class LinkHub : Hub
    {
        public async Task SendLinkUpdate(string linkTitle )
        {
            await Clients.All.SendAsync("ReceiveLinkUpdate", linkTitle);
        }
    }
}
