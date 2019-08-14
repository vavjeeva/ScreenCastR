using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScreenCastApp
{
    public class ScreenCastHub : Hub
    {
        private readonly ScreenCastManager screenCastManager;
        private const string AGENT_GROUP_PREFIX = "AGENT_";

        public ScreenCastHub(ScreenCastManager screenCastManager)
        {
            this.screenCastManager = screenCastManager;
        }

        public async Task AddScreenCastAgent(string agentName)
        {
            await Clients.Others.SendAsync("NewScreenCastAgent", agentName);
            await Groups.AddToGroupAsync(Context.ConnectionId, AGENT_GROUP_PREFIX + agentName);
        }

        public async Task RemoveScreenCastAgent(string agentName)
        {
            await Clients.Others.SendAsync("RemoveScreenCastAgent", agentName);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, AGENT_GROUP_PREFIX + agentName);
            screenCastManager.RemoveViewerByAgent(agentName);
        }

        public async Task AddScreenCastViewer(string agentName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, agentName);
            screenCastManager.AddViewer(Context.ConnectionId, agentName);
            await Clients.Groups(AGENT_GROUP_PREFIX + agentName).SendAsync("NewViewer");
        }

        public async Task RemoveScreenCastViewer(string agentName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, agentName);
            screenCastManager.RemoveViewer(Context.ConnectionId);
            if(!screenCastManager.IsViewerExists(agentName))
                await Clients.Groups(AGENT_GROUP_PREFIX + agentName).SendAsync("NoViewer");
        }

        public async Task StreamCastData(IAsyncEnumerable<string> stream, string agentName)
        {
            await foreach (var item in stream)
            {
                await Clients.Group(agentName).SendAsync("OnStreamCastDataReceived", item);
            }
        }
    }
}
