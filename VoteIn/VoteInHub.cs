using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace VoteIn
{
    public class VoteInHub : Hub
    {
        public async Task VoteAdded(string Guid)
        {
            await Clients.All.SendAsync("VoteAdded" + Guid);
        }
    }
}
